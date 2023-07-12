using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartTracker.Shared.Models
{
    public class ProjectImage : BaseEntity
    {
        [ForeignKey("Id")]
        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }

        [ForeignKey("Id")]
        public int ImageId { get; set; }
        public virtual Document? Image { get; set; }
    }
}
