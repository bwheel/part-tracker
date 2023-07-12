using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartTracker.Shared.Models
{
    public class PartCategory : BaseEntity
    {
        [Required]
        [ForeignKey("Id")]
        public int PartId { get; set; }
        public virtual Part? Part { get; set; }

        [Required]
        [ForeignKey("Id")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
