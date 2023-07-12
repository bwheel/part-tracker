using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartTracker.Shared.Models
{
    public class PartImage : BaseEntity
    {
        [Required]
        [ForeignKey("Id")]
        public int PartId { get; set; }

        public virtual Part? Part { get; set; }

        [Required]
        [ForeignKey("Id")]
        public int ImageId { get; set; }

        public virtual Document? Image { get; set; }
    }
}
