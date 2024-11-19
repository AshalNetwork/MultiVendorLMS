using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UpdateUnit
    {
        public string? Name { get; set; } 

        public double Price { get; set; } = 0;
        public IFormFile? Image { get; set; }

        public Guid CourseId { get; set; }=Guid.Empty;
    }
}
