using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vds.Models
{
    //type of Award
    public class DiseaseType : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DiseaseTypeId { get; set; }
        [Required]
        [Display(Name = "ชื่อโรค")]
        public string Name { get; set; }
       
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "ลำดับ")]
        public int Order { get; set; }
    }
}
