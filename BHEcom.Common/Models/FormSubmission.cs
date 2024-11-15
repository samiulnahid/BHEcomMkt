using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models 
{ 
    public class FormSubmission
    {
        [Key]
        public Guid SubmissionID { get; set; }
        public Guid FormID { get; set; }
        public Guid SubmittedBy { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
