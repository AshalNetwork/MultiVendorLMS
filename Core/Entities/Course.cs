using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Course:BaseEntity
    {
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
        public Guid GradeId { get; set; }
        public Grade Grade { get; set; } = null!;
        public ICollection<Unit> Units { get; set; } = new HashSet<Unit>();
    }
}
