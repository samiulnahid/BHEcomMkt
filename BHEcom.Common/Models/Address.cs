using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Address
    {
        [Key]
        public Guid AddressID { get; set; }
        public Guid UserID { get; set; }
        public string? FullName { get; set; }
        public string? Number { get; set; }
        public string? AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }


        [NotMapped]
        public string? UserName { get; set; }
    }
}
