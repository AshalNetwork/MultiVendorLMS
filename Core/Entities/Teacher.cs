using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Teacher:BaseEntity
    {
        [Length(4, 100, ErrorMessage = "يجب أن يكون الاسم ما بين 4 حروف و 100 حرف")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "يجب ادخال ايميل صحيح")]
        public string Email { get; set; } = null!;

        [Length(6, 300, ErrorMessage = "يجب أن يكون الرفم السري ما بين 6 حروف و 100 حرف")]
        public string Password { get; set; } = null!;

        [Length(11, 11, ErrorMessage = "يجب أن يكون رقم الهاتف 11 رقم")]
        public string Phone { get; set; } = null!;

        [Length(4, 100, ErrorMessage = "يجب أن يكون العنوان ما بين 4 حروف و 100 حرف")]
        public string Address { get; set; } = null!;


        public Guid StageId { get; set; }
        public Stage Stage { get; set; } = null!;

        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();

    }
}
