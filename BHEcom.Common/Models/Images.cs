using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BHEcom.Common.Models
{
    [Table("Images")]
    public class Images
    {
        [Key]
        public Guid ImageId { get; set; }
        public string ImagePath { get; set; }
        public Guid XID { get; set; }
        public string XType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
