using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Shipping
    {
        [Key]
        public Guid ShippingID { get; set; }
        public Guid OrderID { get; set; }
        public Guid ShippingAddressID { get; set; }
        public string ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }
        public string ShippingStatus { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
    }

}
