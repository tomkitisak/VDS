using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vds.Models
{
    //type of Award
    public class JobStatus : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
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
