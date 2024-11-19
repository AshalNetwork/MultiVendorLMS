using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Stage:BaseEntity
    {
        [Length(4, 150, ErrorMessage = "يجب أن يكون الاسم ما بين 4 حروف و 150 حرف")]
        public string Name { get; set; } = null!;

        public ICollection<Grade> Grades { get; set; } = new HashSet<Grade>();
        public ICollection<Teacher> Teachers { get; set; } = new HashSet<Teacher>();

    }
}
