using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    //type of allowance
    public class AllowanceType : Base
    {
        public string AllowanceTypeId { get; set; }
        [Required]
        [Display(Name = "Allowance Type Name")]
        public string Name { get; set; }
        [Display(Name = "Allowance Type Description")]
        public string Description { get; set; }
    }
}
