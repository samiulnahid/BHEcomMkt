using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public Guid ProductID { get; set; }
        public Guid StoreID { get; set; }
        public Guid CategoryID { get; set; }
        public Guid BrandID { get; set; }
        // public Guid SellerID { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string? currencyType { get; set; }
        public decimal? Discount { get; set; }
        public bool? IsTop { get; set; } = false;
        public string? Image { get; set; }


        //// new added
        [NotMapped]
        public string? StoreName { get; set; }
        [NotMapped]
        public string? CategoryName { get; set; }
        [NotMapped]
        public string? BrandName { get; set; }

    }

}
