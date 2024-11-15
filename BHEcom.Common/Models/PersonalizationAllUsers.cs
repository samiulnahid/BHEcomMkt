using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class PersonalizationAllUsers
    {
        [Key]
        public Guid PathId { get; set; }

        public byte[] PageSettings { get; set; }

        public DateTime LastUpdatedDate { get; set; }
    }
}
