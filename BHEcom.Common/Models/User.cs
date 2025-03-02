using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BHEcom.Common.Models
{
    [Table("aspnet_Users")]
    public class User 
    {
        [Key]
        public Guid UserId { get; set; }
        public Guid? ApplicationId { get; set; }
        public string UserName { get; set; }
        public string? LoweredUserName { get; set; }
        public string? MobileAlias { get; set; }
        public bool? IsAnonymous { get; set; } = false;
        public DateTime? LastActivityDate { get; set; }
       

        [NotMapped]
        public string? RoleName { get; set; }
        [NotMapped]
        public string? Password { get; set; }
        [NotMapped]
        public string? Email { get; set; }
        [NotMapped]
        public string? PasswordSalt { get; set; }
        [NotMapped]
        public DateTime? LoginDate { get; set; }
        [NotMapped]
        public bool? IsLogin { get; set; } = false;
        [NotMapped]
        public DateTime? CreateDate { get; set; }
        [NotMapped]
        public DateTime? UpdateDate { get; set; }
    }
}
