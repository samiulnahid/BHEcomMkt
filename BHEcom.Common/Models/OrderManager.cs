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
        public Guid CartID { get; set; }


      //  public IEnumerable<CartManager>? CartProductList { get; set; }
    }
}
