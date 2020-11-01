using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    //type of appraisal
    public class AppraisalType : Base
    {
        public string AppraisalTypeId { get; set; }
        [Required]
        [Display(Name = "Appraisal Type Name")]
        public string Name { get; set; }
        [Display(Name = "Appraisal Type Description")]
        public string Description { get; set; }
    }
}
