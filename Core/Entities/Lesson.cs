using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Lesson:BaseEntity
    {
        [Length(4, 150, ErrorMessage = "يجب أن يكون الاسم ما بين 4 حروف و 150 حرف")]
        public string Name { get; set; } = null!;

        [Length(4, 350, ErrorMessage = "يجب أن يكون الاسم ما بين 4 حروف و 350 حرف")]
        public string About { get; set; } = null!;

        [Range(20, double.MaxValue, ErrorMessage = "يجب ألا يقل السعر عن 20 جنيه")]
        public double Price { get; set; }

        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }=null!;

        public ICollection<LessonDetail> LessonDetails { get; set; } = new HashSet<LessonDetail>();

    }
}
