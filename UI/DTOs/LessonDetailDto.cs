using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class LessonDetailDto
    {
        public Guid LessonId { get; set; }
        public IFormFile Video { get; set; } = null!;
        public IFormFile? Thumbnail { get; set; } 
        [MinLength(4)]
        public string Title { get; set; } = null!;

    }
}
