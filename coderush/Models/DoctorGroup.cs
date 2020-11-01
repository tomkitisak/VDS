using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    public class DoctorGroup : Base
    {
        public string DoctorGroupId { get; set; }

        [Required]
        [Display(Name = "ชื่อกลุ่มแพทย์")]
        public string DoctorGroupName { get; set; }

        [Required]
        [Display(Name = "ความเชียวชาญของแพทย์ในกลุ่ม")]
        public string DoctorTypeId { get; set; }
        public DoctorType DoctorType { get; set; }

     

    }
}
