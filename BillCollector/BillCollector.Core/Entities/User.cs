using System.ComponentModel.DataAnnotations;
using BillCollector.Core.Entities;
using BillCollector.Core.Enums;

namespace BillCollector.Core.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
        }


        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Status { get; set; } = UserStatus.PENDING.ToString();

        public bool EmailVerified { get; set; }
        public bool PhoneVerified { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int FailedLoginAttempts { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetExpires { get; set; }


        // Navigation properties
        public virtual UserRole UserRole { get; set; }
    }
}
