using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.FileServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController(IUnitOfWork unitOfWork,
                                 IFileConverterService fileConverter,
                                 AppDbContext context) : ControllerBase
    {
        [HttpGet("GetUnits")]
        public async Task<IActionResult> GetAll(CancellationToken token) =>
                Ok(await unitOfWork.Repository().FindAllAsync<Unit>(token));

        [HttpGet("GetUnit/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var Unit = await unitOfWork.Repository().GetById<Unit>(id);
            if (Unit == null)
                return NotFound();
            return Ok(Unit);
        }

        [HttpPost("AddUnit")]
        public async Task<IActionResult> Create([FromForm] UnitDto dto,CancellationToken token)
        {
            if ((await unitOfWork.Repository().GetById<Course>(dto.CourseId)) == null)
                return BadRequest("لا يوجد هذا الكورس");
            var Unit = new Unit
            {
                Name = dto.Name,
                CourseId = dto.CourseId,
                Image = await fileConverter.Convert(dto.Image),
                Price = dto.Price,
            };
            await unitOfWork.Repository().Add(Unit);
            await unitOfWork.CommitAsync(token);    
            return Ok(Unit.Id);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateUnit updateUnit,CancellationToken token)
        {
            var unit = await unitOfWork.Repository().GetById<Unit>(id);
            if (unit is null)
                return NotFound("لاتوجد هذه الوحدة");
            unitOfWork.Repository().Update(unit);
            await unitOfWork.CommitAsync(token);
            await context.SaveChangesAsync();
            return Ok("تم التعديل ");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id,CancellationToken token)
        {
            var unit = await unitOfWork.Repository().GetById<Unit>(id);
            if (unit != null)
            {
                unitOfWork.Repository().Delete(unit);
                await unitOfWork.CommitAsync(token);
                return Ok("تم الحذف بنجاح");
            }
            return NoContent();
        }
    }
}
