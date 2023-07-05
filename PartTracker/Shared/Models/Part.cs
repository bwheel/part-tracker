using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartTracker.Shared.Models
{
    public class Part : BaseEntity
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = string.Empty;
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
    }
}
