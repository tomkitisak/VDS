using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    //type of asset
    public class PrefixType : Base
    {
        public string PrefixTypeId { get; set; }
        [Required]
        [Display(Name = "ลำดับการแสดงผล")]
        public int Order { get; set; }

        [Required]
        [Display(Name = "คำนำหน้าชื่อ")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "คำนำหน้าชื่อ(เต็ม)")]
        public string Description { get; set; }
    }
}
