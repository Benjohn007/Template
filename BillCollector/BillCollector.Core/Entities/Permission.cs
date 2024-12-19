using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using BillCollector.Core.Entities;

namespace BillCollector.Core.Entities
{
    public class Permission : BaseEntity
    {
        public long RoleId { get; set; }
        public string PermissionName { get; set; }

        // Navigation properties
        public virtual Role Role { get; set; }
    }
}
