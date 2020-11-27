using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vds.Models
{
    public class DoctorType : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        public string DoctorTypeId { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลสาขาความเชี่ยวชาญ")]
        [Display(Name = "สาขาความเชี่ยวชาญ")]
        public string DoctorTypeName { get; set; }

      
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลลำกับ")]
        [Display(Name = "ลำดับ")]
        public int Order { get; set; }
    }
}
