using API.DTOs;
using Azure.Core;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Drawing;
using System.Net.Http.Headers;
using Stripe;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController(IUnitOfWork unitOfWork,
                                   AppDbContext context,
                                   IConfiguration configuration) : ControllerBase
    {
        [HttpGet("GetLessons")]
        public async Task<IActionResult> GetAll(CancellationToken token) =>
               Ok(await unitOfWork.Repository().FindAllAsync<Lesson>(token));

        [HttpGet("GetLesson/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var Lesson = await unitOfWork.Repository().GetById<Lesson>(id);
            if (Lesson == null)
                return NotFound();
            return Ok(Lesson);
        }

        [HttpGet("GetLessonDetails/{id}")]
        public async Task<IActionResult> GetLessonDetails(Guid id)
        {
            var res = await context.LessonDetails.Where(e => e.LessonId == id).Select(e => new GetLessonDetailDto { Title = e.Title, LessonId = e.LessonId, VideoId = e.VideoId }).ToListAsync();
            foreach (var item in res)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://video.bunnycdn.com/library/{configuration.GetSection("BunnyStream")["LibId"]}/videos/{item.VideoId}"),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "AccessKey", configuration.GetSection("BunnyStream")["ApiKey"] },
                    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);

                    item.Views = int.Parse(jsonResponse["views"].ToString());
                }
            }
            return Ok(res);
        }

        [HttpGet("GetLessonsByUnitId/{id}")]
        public async Task<IActionResult> GetLessonsByUnitId(Guid id) =>
                Ok(await context.Lessons.Where(e=>e.UnitId==id).ToListAsync());


        [HttpPost("AddLesson")]
        public async Task<IActionResult> Create([FromForm] LessonDto dto,CancellationToken token)
        {
            if ((await unitOfWork.Repository().GetById<Unit>(dto.UnitId)) == null)
                return BadRequest("لا توجد هذه الوحدة");
            var Lesson = new Lesson
            {
                Name = dto.Name,
                UnitId = dto.UnitId,
                About = dto.About,
                Price = dto.Price,
            };
            await unitOfWork.Repository().Add(Lesson);
            await unitOfWork.CommitAsync(token);
            return Ok(Lesson.Id);
        }

        [HttpPost("AddLessonDetails")]
        public async Task<IActionResult> AddLessonDetails([FromForm] LessonDetailDto dto)
        {
            if ((await unitOfWork.Repository().GetById<Lesson>(dto.LessonId)) == null)
                return BadRequest("لا توجد هذه الوحدة");
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://video.bunnycdn.com/library/{configuration.GetSection("BunnyStream")["LibId"]}/videos"),
                Headers =
                {
                    { "accept", "application/json" },
                    { "AccessKey", configuration.GetSection("BunnyStream")["ApiKey"] },
                },
                Content = new StringContent($"{{\"title\":\"{dto.Title}\"}}")
                {
                    Headers =
                    {
                        ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")
                    }

                }
            };
            string videoId = "";
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
                videoId = jsonResponse["guid"].ToString();
            }
            var uploadUrl = $"https://video.bunnycdn.com/library/{configuration.GetSection("BunnyStream")["LibId"]}/videos/{videoId}";
            using (var fileStream = dto.Video.OpenReadStream())
            {
                var content = new StreamContent(fileStream);
                content.Headers.ContentType = new MediaTypeHeaderValue(dto.Video.ContentType);

                var requestMessage = new HttpRequestMessage(HttpMethod.Put, uploadUrl)
                {
                    Content = content
                };
                requestMessage.Headers.Add("AccessKey", configuration.GetSection("BunnyStream")["ApiKey"]);

                var response = await client.SendAsync(requestMessage);
            }
            var lesson = new LessonDetail
            {
                LessonId = dto.LessonId,
                VideoId =new Guid( videoId),
                Title=dto.Title 
            };

            await context.LessonDetails.AddAsync(lesson);
            await context.SaveChangesAsync();
            if (dto.Thumbnail != null)
            {
                // Construct the URL for uploading the thumbnail
                var url = $"https://video.bunnycdn.com/library/{configuration.GetSection("BunnyStream")["LibId"]}/videos/{videoId}/thumbnail";
                using (var fileStream = dto.Thumbnail.OpenReadStream())
                {
                    var content = new StreamContent(fileStream);
                    content.Headers.ContentType = new MediaTypeHeaderValue(dto.Thumbnail.ContentType);

                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = content
                    };
                    // Note: Adjust headers as necessary based on API requirements
                    requestMessage.Headers.Add("AccessKey", configuration.GetSection("BunnyStream")["ApiKey"]);

                    var response = await client.SendAsync(requestMessage);

                }
            }
            return Ok(lesson.VideoId);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateLessonDto updateLesson,CancellationToken token)
        {
            var Lesson = await unitOfWork.Repository().GetById<Lesson>(id);
            if (Lesson != null)
            {
                unitOfWork.Repository().Update(Lesson);
                await unitOfWork.CommitAsync(token);
            }
            return Ok("تم التعديل ");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id,CancellationToken token)
        {
            var lesson = await unitOfWork.Repository().GetById<Lesson>(id);
            if (lesson != null)
            {
                unitOfWork.Repository().Delete(lesson);
                await unitOfWork.CommitAsync(token);
                return Ok("تم الحذف بنجاح");
            }
            return NoContent();
        }

        [HttpPost("create-payment-intent")]
        public IActionResult CreatePaymentIntent([FromBody] long amount)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "egp", // You can change this depending on your business needs
                    PaymentMethodTypes = new List<string> { "card" }
                };

                var service = new PaymentIntentService();
                PaymentIntent intent = service.Create(options);

                return Ok(new { clientSecret = intent.ClientSecret });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }

}
