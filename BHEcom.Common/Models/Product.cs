using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Product
    {
        [Key]
        public Guid ProductID { get; set; }
        public Guid StoreID { get; set; }
        public Guid CategoryID { get; set; }
        public Guid BrandID { get; set; }
        public Guid SellerID { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Image { get; set; }


        //// new added
        //public string SellerName { get; set; }
        //public Guid? OwnerID { get; set; }
        //public Guid? OwnerName { get; set; }
        //public string CategoryName { get; set; }
        //public string BrandName { get; set; }

    }

}
