using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class Attributes
    {
        [Key]
        public Guid AttributeID { get; set; }
        public string AttributeName { get; set; }
        public string EntityName { get; set; }
        public string EntityValue { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid XId { get; set; }
        public string XType { get; set; }
    }

}
