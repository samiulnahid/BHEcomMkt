using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class WebEvent
    {
        [Key]
        public string EventId { get; set; }

        [Required]
        public DateTime EventTimeUtc { get; set; }

        [Required]
        public DateTime EventTime { get; set; }

        [Required]
        public string EventType { get; set; }

        [Required]
        public decimal EventSequence { get; set; }

        [Required]
        public decimal EventOccurrence { get; set; }

        [Required]
        public int EventCode { get; set; }

        [Required]
        public int EventDetailCode { get; set; }

        [StringLength(1024)]
        public string Message { get; set; }

        [StringLength(256)]
        public string ApplicationPath { get; set; }

        [StringLength(256)]
        public string ApplicationVirtualPath { get; set; }

        [Required]
        [StringLength(256)]
        public string MachineName { get; set; }

        [StringLength(1024)]
        public string RequestUrl { get; set; }

        [StringLength(256)]
        public string ExceptionType { get; set; }

        [Column(TypeName = "ntext")]
        public string Details { get; set; }
    }
}
