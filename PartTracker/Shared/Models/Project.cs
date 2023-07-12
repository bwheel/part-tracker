using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartTracker.Shared.Models
{
    public class Project : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<Part>? Parts { get; set;}
        public virtual ICollection<ProjectCategory>? ProjectCategories { get; set; }
        public virtual ICollection<ProjectImage>? ProjectImages { get; set; }
        public virtual ICollection<ProjectAttachment>? ProjectAttachments { get; set; }
    }
}
