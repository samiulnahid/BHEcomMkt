using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("aspnet_UsersInRoles")]
    public class UsersInRole
    {
     
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        [NotMapped]
        public Guid? ApplicationId { get; set; }
    }
}
