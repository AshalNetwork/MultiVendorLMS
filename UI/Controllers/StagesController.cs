using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StagesController(IUnitOfWork unitOfWork) : ControllerBase
    {

        [HttpGet("GetStages")]
        public async Task<IActionResult> GetAll(CancellationToken token) =>
            Ok(await unitOfWork.Repository().FindAllAsync<Stage>(token));

        [HttpGet("GetStage/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var stage = await unitOfWork.Repository().GetById<Stage>(id);
            if (stage == null)
                return NotFound();
            return Ok(stage);
        }

        [HttpPost("AddStage")]
        public async Task<IActionResult> Create([FromForm] string name,CancellationToken token)
        {
            var stage = new Stage
            {
                Name = name,
            };
            await unitOfWork.Repository().Add(stage);
            await unitOfWork.CommitAsync(token);
            return Ok(stage.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id,CancellationToken token, [FromForm] string? name)
        {
            var stage = await unitOfWork.Repository().GetById<Stage>(id);
            if(stage is null)
                return NotFound("لا توجد هذه المرحلة");  
            if(name != null)
            {
                stage.Name = name;
                unitOfWork.Repository().Update(stage);
                await unitOfWork.CommitAsync(token);
                return Ok("تم التعديل بنجاح");
            }
            return Ok("لم يتم تعديل اي شئ");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id,CancellationToken token)
        {
            var stage = await unitOfWork.Repository().GetById<Stage>(id);
            if (stage != null)
            {
                unitOfWork.Repository().Delete(stage);
                await unitOfWork.CommitAsync(token);
                return Ok("تم الحذف بنجاح");
            }
            return NoContent();
        }
    }
}