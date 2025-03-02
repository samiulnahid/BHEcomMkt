using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Order
    {
        [Key]
        public Guid OrderID { get; set; }
        public Guid? UserID { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Status { get; set; }
        public Guid? ShippingAddressID { get; set; }
        public Guid? BillingAddressID { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TotalAfterDiscount { get; set; }
        public Guid? CouponId { get; set; }
        public decimal TotalDeliveryFee { get; set; } = 0;
        public string OrderNumber { get; set; }

        [NotMapped]
        public Guid CartID { get; set; }
    }
}
