using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    //type of Award
    public class JobStatus : Base
    {
        public string JobStatusId { get; set; }
        [Required]
        [Display(Name = "สถานะ")]
        public int Status { get; set; }
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }

        public bool  AllowAddDoctor { get; set; }
        public bool AllowAddPatient { get; set; }
    }
}
