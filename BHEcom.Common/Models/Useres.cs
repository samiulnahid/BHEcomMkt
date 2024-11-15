using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Useres
    {
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public DateTime? LoginDate { get; set; }
        public bool IsLogin { get; set; } = false;
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }


    }
}
