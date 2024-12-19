using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public virtual long Id { get; set; }
        public string CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
