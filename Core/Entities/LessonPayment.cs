using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class LessonPayment
    {
        public Guid LessonId { get; set; }
        public Guid StudentId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public Student Student { get; set; } = null!;
    }
}
