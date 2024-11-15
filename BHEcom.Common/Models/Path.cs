using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class AspnetPath
    {
        [Key]
        public Guid PathId { get; set; }

        public Guid ApplicationId { get; set; }
        public string Path { get; set; }
        public string LoweredPath { get; set; }
    }
}
