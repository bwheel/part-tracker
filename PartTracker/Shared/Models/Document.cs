using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartTracker.Shared.Models
{
    public class Document : BaseEntity
    {
        [Required(AllowEmptyStrings = false)]
        public string Filename { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string ServerDirPath { get; set; } = string.Empty;

        [Required]
        public long SizeBytes { get; set; } = 0;

        [Required]
        public DateTime Uploaded { get; set; } = DateTime.UtcNow;

        public virtual ICollection<PartImage>? PartImages { get; set; }
        public virtual ICollection<ProjectImage>? ProjectImages { get; set; }
        public virtual ICollection<PartAttachment>? PartAttachments { get; set; }
        public virtual ICollection<ProjectAttachment>? ProjectAttachments { get; set; }
    }
}
