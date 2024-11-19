using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Unit:BaseEntity
    {
        [Length(4, 50, ErrorMessage = "يجب أن يكون الاسم ما بين 4 حروف و 50 حرف")]
        public string Name { get; set; } = null!;

        [Range(20,double.MaxValue,ErrorMessage ="يجب ألا يقل السعر عن 20 جنيه")]
        public double Price { get; set; }

        [MaxLength(int.MaxValue)]
        public string Image { get; set; } = null!;

            
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}
