using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UpdateLessonDto
    {
        public string? Name { get; set; } 
        public string? About { get; set; }
        public double Price { get; set; } = 0;
    }
}
