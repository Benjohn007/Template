using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillCollector.Core.Entities;
using BillCollector.Core.Enums;

namespace BillCollector.Application.Dto.Auth
{
    public class UserDto
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Status { get; set; }

        public bool EmailVerified { get; set; }
        public bool PhoneVerified { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string Role { get; set; }
    }
}
