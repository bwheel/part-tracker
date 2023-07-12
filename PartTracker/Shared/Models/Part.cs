using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartTracker.Shared.Models
{
    public class Part : BaseEntity
    {
        [Required]
        [ForeignKey("Id")]
        public int ProjectId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; } = string.Empty;
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [DefaultValue(1)]
        public int Quantity { get; set; } = 1;
        public string? SerialNumber { get; set; }
        public string? ModelNumber { get; set; }
        public string? Brand { get; set; }
        public string? Notes { get; set; }

        public virtual Project? PartCollection { get; set; }
        public virtual ICollection<PartCategory>? PartCategories { get; set; }
        public virtual ICollection<PartImage>? PartImages { get; set; }
        public virtual ICollection<PartAttachment>? PartAttachments { get; set; }
    }
}
