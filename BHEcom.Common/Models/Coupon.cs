using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("Coupons")]
    public class Coupon
    {
        [Key]
        public Guid CouponId { get; set; }
        public string Code { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool? IsActive { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? StoreId { get; set; }
    }
}
