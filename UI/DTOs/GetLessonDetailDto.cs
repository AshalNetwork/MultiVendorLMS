using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class GetLessonDetailDto
    {
        public Guid LessonId { get; set; }
        public Guid VideoId { get; set; } 
        public string Title { get; set; } = null!;
        public int Views { get; set; }
    }
}
