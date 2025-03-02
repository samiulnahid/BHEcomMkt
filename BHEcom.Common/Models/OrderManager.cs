using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class OrderManager
    {
        public Guid? OrderID { get; set; }
        public Guid? UserID { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public Guid? ShippingAddressID { get; set; }
        public Guid? BillingAddressID { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TotalAfterDiscount { get; set; }
        public Guid? CouponId { get; set; }
        public decimal? TotalDeliveryFee { get; set; }
        public string? OrderNumber { get; set; }
        public Guid? OrderDetailID { get; set; }
        public int? Quantity { get; set; }
        public decimal? OrderDetailPrice { get; set; }
        public Guid? ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? Stock { get; set; }
        public string? Image { get; set; }
        public string? ShortDescription { get; set; }
        public decimal? Discount { get; set; }
        public Guid? StoreID { get; set; }
        public string? StoreName { get; set; }


        //  public IEnumerable<CartManager>? CartProductList { get; set; }
    }
}
