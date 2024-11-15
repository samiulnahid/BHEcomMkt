using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class PersonalizationPerUser
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PathId { get; set; }
        public Guid UserId { get; set; }
        public byte[] PageSettings { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
