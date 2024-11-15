using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Store
    {
        [Key]
        public Guid StoreID { get; set; } 
        public Guid OwnerID { get; set; } 
        public string? StoreName { get; set; } 
        public string? Description { get; set; } 
        public DateTime? CreatedDate { get; set; } 
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
