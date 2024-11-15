using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class StoreManager
    {
        // Store Field
        public Guid StoreID { get; set; }
        public Guid OwnerID { get; set; }
        public string? StoreName { get; set; }
        public string? Description { get; set; }


        // Agent Field
        public Guid AgentID { get; set; }
        public string AgencyName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }


        // address field
        public Guid AddressID { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        // User Field
        public Guid UserId { get; set; }
        public string? UserName { get; set; }

    }
    public class StoreConfig
    {
        public Guid StoreID { get; set; }
        public List<StoreBrand>? StoreBrands { get; set; } 
        public List<StoreCategory>? StoreCategories { get; set; } 
    }
    public class StoreProductManager
    {
        public List<StoreProductField>? StoreProductField { get; set; } 
    }

    public class CategoryFieldsDto
    {
        public Guid CategoryID { get; set; }
        public Guid StoreCategoryID { get; set; }
        public List<string> Fields { get; set; }
    }

}
