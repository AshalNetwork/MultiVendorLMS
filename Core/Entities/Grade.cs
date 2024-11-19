using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Grade : BaseEntity
    {
        [Length(4,50,ErrorMessage ="يجب أن يكون الاسم ما بين 4 حروف و 50 حرف")]
        public string Name { get; set; } = null!;

        public Guid StageId { get; set; }
        public Stage Stage { get; set; } = null!;

        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
