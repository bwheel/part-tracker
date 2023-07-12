using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PartTracker.Shared.Models
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<PartCategory>? PartCategories { get; set; }
        public virtual ICollection<ProjectCategory>? ProjectCategories { get; set; }
    }
}
