using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UnitDto
    {
        [Length(4, 50, ErrorMessage = "يجب أن يكون الاسم ما بين 4 حروف و 50 حرف")]
        public string Name { get; set; } = null!;

        [Range(20, double.MaxValue, ErrorMessage = "يجب ألا يقل السعر عن 20 جنيه")]
        public double Price { get; set; }


        public IFormFile Image { get; set; } = null!;

        public Guid CourseId { get; set; }
    }
}
