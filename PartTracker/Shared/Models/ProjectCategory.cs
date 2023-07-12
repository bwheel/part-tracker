using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartTracker.Shared.Models
{
    public class ProjectCategory : BaseEntity
    {
        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
