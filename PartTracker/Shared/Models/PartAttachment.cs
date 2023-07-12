using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartTracker.Shared.Models
{
    public class PartAttachment : BaseEntity
    {
        [Required]
        [ForeignKey("Id")]
        public int PartId { get; set; }

        public virtual Part? Part { get; set; }

        [Required]
        [ForeignKey("Id")]
        public int AttachmentId { get; set; }
        public virtual Document? Attachment { get; set; }
    }
}
