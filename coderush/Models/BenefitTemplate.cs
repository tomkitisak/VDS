using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    public class BenefitTemplate : Base
    {
        public string BenefitTemplateId { get; set; }
        [Required]
        [Display(Name = "Benefit Template Name")]
        public string Name { get; set; }
        [Display(Name = "Benefit Template Description")]
        public string Description { get; set; }

        //lines
        public List<BenefitTemplateLine> Lines { get; set; } = new List<BenefitTemplateLine>();
    }
}
