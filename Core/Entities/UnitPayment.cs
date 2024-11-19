using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UnitPayment
    {
        public Guid UnitId  { get; set; }
        public Guid StudentId  { get; set; }
        public Unit Unit { get; set; } = null!;
        public Student Student { get; set; } = null!;
    }
}
