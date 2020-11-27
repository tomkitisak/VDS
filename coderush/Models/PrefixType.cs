using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vds.Models
{
    //type of asset
    public class PrefixType : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
