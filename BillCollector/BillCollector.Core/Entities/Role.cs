using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillCollector.Core.Entities;

namespace BillCollector.Core.Entities
{
    public class Role : BaseEntity
    {
        public Role()
        {
        }


        public string Name { get; set; }

        public string Description { get; set; }

        // Navigation properties
        public virtual UserRole UserRole { get; set; }
        public virtual ICollection<Permission> RolePermissions { get; set; }
    }
}
