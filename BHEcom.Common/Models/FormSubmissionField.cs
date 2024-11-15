using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHEcom.Common.Models
{
    public class FormSubmissionField
    {
        [Key]
        public Guid SubmissionFieldID { get; set; }
        public Guid SubmissionID { get; set; }
        public Guid FieldID { get; set; }

        [MaxLength] 
        public string FieldValue { get; set; }
    }
}
