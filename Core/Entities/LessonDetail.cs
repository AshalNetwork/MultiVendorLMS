using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public  class LessonDetail
    {
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public Guid VideoId { get; set; }
        public string Title { get; set; } = null!;
    }
}
