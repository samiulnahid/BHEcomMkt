using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class DeliveryDetails
    {
        [Key]
        public Guid DeliveryID { get; set; }

        [Required]
        public Guid OrderID { get; set; }

        [Required]
        public Guid StoreID { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DeliveryFee { get; set; }

        [Required]
        [StringLength(50)]
        public string DeliveryStatus { get; set; } = "Pending"; // Default value

        [Required]
        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
