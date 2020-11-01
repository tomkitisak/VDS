using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    //type of allowance
    public class JobDoctor : Base
    {

        public string JobDoctorId { get; set; }

        [Required]
        [Display(Name = "งาน")]
        public string JobId { get; set; }

        public Job Job { get; set; }

        [Required]
        [Display(Name = "แพทย์")]
        public string DoctorId { get; set; }

        public Doctor Doctor { get; set; }
 

    }
}
