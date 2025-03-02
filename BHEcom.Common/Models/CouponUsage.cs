using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("CouponUsages")]
    public class CouponUsage
    {
        [Key]
        public Guid CouponUsageID { get; set; }

        public Guid CouponId { get; set; }

        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public DateTime? UsedAt { get; set; }
    }
}
