using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    //type of Award
    public class Gender : Base
    {
        public string GenderId { get; set; }
        [Required]
        [Display(Name = "เพศ")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "ลำดับ")]
        public int Order { get; set; }
    }
}
