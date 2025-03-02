using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    [Table("PageVisits")]
    public class PageVisit
    {
        [Key]
        public Guid PageVisitID { get; set; }
        public Guid XID { get; set; }
        public string XType { get; set; }
        public string Month { get; set; }
        public int Hit { get; set; }
    }
}
