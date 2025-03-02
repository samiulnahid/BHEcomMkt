using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("aspnet_Membership")]
    public class Membership
    {
        [Key]
        //public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Password { get; set; } 
        public int? PasswordFormat { get; set; } = 0;
        public string? PasswordSalt { get; set; } = null;
        public string? MobilePIN { get; set; } = null;
        public string? Email { get; set; }
        public string? LoweredEmail { get; set; } = null;
        public string? PasswordQuestion { get; set; } = null;
        public string? PasswordAnswer { get; set; } = null;
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime? CreateDate { get; set; } = null;
        public DateTime? LastLoginDate { get; set; } = null;
        public DateTime? LastPasswordChangedDate { get; set; } = null;
        public DateTime? LastLockoutDate { get; set; } = null;
        public int? FailedPasswordAttemptCount { get; set; } = 0;
        public DateTime? FailedPasswordAttemptWindowStart { get; set; } = null;
        public int? FailedPasswordAnswerAttemptCount { get; set; } = 0;
        public DateTime? FailedPasswordAnswerAttemptWindowStart { get; set; } = null;
        public string? Comment { get; set; } = null;
    }
}
