using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("DeliveryLogs")]
    public class DeliveryLog
    {
        [Key]
        public Guid LogID { get; set; }
        public Guid DeliveryID { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Remarks { get; set; }
    }

}
