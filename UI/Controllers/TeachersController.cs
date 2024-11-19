using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.PasswordServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController(IUnitOfWork unitOfWork,
                                    AppDbContext context) : ControllerBase
    {
        [HttpGet("GetTeachers")]
        public async Task<IActionResult> GetAll(CancellationToken token) =>
            Ok(await unitOfWork.Repository().FindAllAsync<Teacher>(token));

        [HttpGet("GetTeacher/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var Teacher = await unitOfWork.Repository().GetById<Teacher>(id);
            if (Teacher == null)
                return NotFound();
            return Ok(Teacher);
        }

        [HttpPost("AddTeacher")]
        public async Task<IActionResult> Create([FromForm] TeacherDTO dto,CancellationToken token)
        {
            if((await unitOfWork.Repository().GetById<Stage>(dto.StageId))==null)
                return BadRequest("لا توجد مرحلة");
            var hashPassword = PasswordHasher.HashPassword(dto.Password, out byte[] salt);
            var teacher = new Teacher
            {
                Name = dto.Name,
                StageId = dto.StageId,
                Phone=dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                Password= hashPassword,
            };
            await unitOfWork.Repository().Add(teacher);
            var grades = context.Grades.Where(e => e.StageId == dto.StageId).ToList();
            for (int i=0;i< grades.Count; i++)
            {
                await unitOfWork.Repository().Add(new Course
                {
                    GradeId= grades[i].Id,
                    TeacherId=teacher.Id
                });
            }
            await unitOfWork.CommitAsync(token);
            return Ok(teacher.Id);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CancellationToken token,[FromForm] string? name)
        {
            var Teacher = await unitOfWork.Repository().GetById<Teacher>(id);
            if (Teacher is null)
                return NotFound("لا توجد هذه المرحلة");
            if (name != null)
            {
                Teacher.Name = name;
                 unitOfWork.Repository().Update(Teacher);
                await unitOfWork.CommitAsync(token);
                return Ok("تم التعديل بنجاح");
            }
            return Ok("لم يتم تعديل اي شئ");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id,CancellationToken token)
        {
            var teacher = await unitOfWork.Repository().GetById<Teacher>(id);
            if (teacher != null)
            {
                unitOfWork.Repository().Delete(teacher);
                await unitOfWork.CommitAsync(token);
                return Ok("تم الحذف بنجاح");
            }
            return NoContent();
        }
    }
}
