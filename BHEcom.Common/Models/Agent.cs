using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Agent
    {
        [Key]
        public Guid AgentID { get; set; }
        public Guid UserID { get; set; }
        public string AgencyName { get; set; }
        public string ContactPerson { get; set; }
        public string? ContactEmail { get; set; }
        public string ContactPhone { get; set; }

        [NotMapped]
        public string? UserName { get; set; }
    }

}
