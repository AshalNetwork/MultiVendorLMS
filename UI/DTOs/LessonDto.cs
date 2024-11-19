using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class LessonDto
    {
        [Length(4, 150, ErrorMessage = "يجب أن يكون الاسم ما بين 4 حروف و 150 حرف")]
        public string Name { get; set; } = null!;

        [Length(4, 350, ErrorMessage = "يجب أن يكون الاسم ما بين 4 حروف و 350 حرف")]
        public string About { get; set; } = null!;

        [Range(20, double.MaxValue, ErrorMessage = "يجب ألا يقل السعر عن 20 جنيه")]
        public double Price { get; set; }

        public Guid UnitId { get; set; }
    }
}
