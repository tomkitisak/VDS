using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    //type of Award
    public class DiseaseType : Base
    {
        public string DiseaseTypeId { get; set; }
        [Required]
        [Display(Name = "ชื่อโรค")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "ลำดับ")]
        public int Order { get; set; }
    }
}
