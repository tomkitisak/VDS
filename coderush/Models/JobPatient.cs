using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vds.Models
{
    //type of allowance
    public class JobPatient : Base
    {
        public string JobPatientId { get; set; }

        [Required]
        [Display(Name = "งาน")]
        public string JobId { get; set; }
        public Job Job { get; set; }

       
        [Display(Name = "คนไข้")]
        public string PatientId { get; set; }

        public Patient Patient { get; set; }

        // navigation properties

 
        
    }
}
