using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet("GetGrades")]
        public async Task<IActionResult> GetAll(CancellationToken token) =>
            Ok(await unitOfWork.Repository().FindAllAsync<Grade>(token));

        [HttpGet("GetGrade/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var Grade = await unitOfWork.Repository().GetById<Grade>(id);
            if (Grade == null)
                return NotFound();
            return Ok(Grade);
        }

        [HttpPost("AddGrade")]
        public async Task<IActionResult> Create([FromForm] string name, [FromForm] Guid stageId,CancellationToken token)
        {
            if ((await unitOfWork.Repository().GetById<Stage>(stageId))== null)
                return BadRequest("لا توجد مرحلة بهذا الرقم");
            var Grade = new Grade
            {
                Name = name,
                StageId=stageId
            };
            await unitOfWork.Repository().Add(Grade);
            await unitOfWork.CommitAsync(token);
            return Ok(Grade.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] string? name)
        {
            var Grade = await unitOfWork.Repository().GetById<Grade>(id);
            if (Grade is null)
                return NotFound("لا يوجد هذا الصف");
            if (name != null)
            {
                Grade.Name = name;
                unitOfWork.Repository().Update(Grade);
                return Ok("تم التعديل بنجاح");
            }
            return Ok("لم يتم تعديل اي شئ");
        }

        [HttpDelete("DeleteGrade{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var grade = await unitOfWork.Repository().GetById<Grade>(id);
            if (grade != null)
            {
                unitOfWork.Repository().Delete(grade);
                return Ok("تم الحذف بنجاح");
            }
            return NoContent();
        }
    }
}
