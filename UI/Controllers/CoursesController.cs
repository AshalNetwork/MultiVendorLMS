using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController(IUnitOfWork unitOfWork,
                                   AppDbContext context) : ControllerBase
    {
        [HttpGet("GetCourses")]
        public async Task<IActionResult> GetAll(CancellationToken token) =>
           Ok(await unitOfWork.Repository().FindAllAsync<Course>( token));

        [HttpGet("GetCourse/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var Course = await unitOfWork.Repository().GetById<Course>(id);
            if (Course == null)
                return NotFound();
            return Ok(Course);
        }

        [HttpGet("GetCoursesByTeacherId/{id}")]
        public async Task<IActionResult> GetCoursesByTeacherId(Guid id)
        {
            
            return Ok(await context.Grades.Where(e => e.Courses.Any(e => e.TeacherId == id)).Select(e=>new {Id=e.Id,Name=e.Name }).ToListAsync());
        }

        [HttpPost("AddCourse")]
        public async Task<IActionResult> Create([FromForm] CourseDto dto,CancellationToken token)
        {
            if (await unitOfWork.Repository().GetById<Grade>(dto.GradeId) == null)
                return BadRequest("لا توجد مرحلة");
            if ((await unitOfWork.Repository().GetById<Teacher>(dto.TeacherId)) == null)
                return BadRequest("لا يوجد هذا المدرس");
            var Course = new Course
            {
                TeacherId = dto.TeacherId,
                GradeId = dto.GradeId,
            };
            await unitOfWork.Repository().Add<Course>(Course);
            await unitOfWork.CommitAsync(token);
            return Ok(Course.Id);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var course = await unitOfWork.Repository().GetById<Course>(id);
            if (course != null)
            {
                unitOfWork.Repository().Delete(course);
                return Ok("تم الحذف بنجاح");
            }
            return NoContent();
        }
    }
}
