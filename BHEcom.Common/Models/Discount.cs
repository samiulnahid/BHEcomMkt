using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Discount
    {
        [Key]
        public Guid DiscountID { get; set; } 
        public Guid? StoreID { get; set; }
        public string DiscountType { get; set; } 
        public decimal DiscountValue { get; set; } 
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
        public string EntityName { get; set; } 
        public Guid EntityID { get; set; } 
        public bool? IsActive { get; set; }
    }
}
