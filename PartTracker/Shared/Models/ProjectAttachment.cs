using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartTracker.Shared.Models
{
    public class ProjectAttachment : BaseEntity
    {
        [ForeignKey("Id")]
        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public int AttachmentId { get; set; }
        public virtual Document? Attachment { get; set; }
    }
}
