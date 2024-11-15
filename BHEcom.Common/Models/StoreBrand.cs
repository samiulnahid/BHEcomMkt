using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class StoreBrand
    {
        [Key]
        public Guid StoreBrandID { get; set; }
        public Guid StoreID { get; set; }
        public Guid BrandID { get; set; }
        public bool IsActive { get; set; } 

        [NotMapped]
        public string? BrandName { get; set; }
    }
}
