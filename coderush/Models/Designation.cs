using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    // official position of an employee
    public class Designation : Base
    {
        public string DesignationId { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลชื่อตำแหน่งงาน")]
        [Display(Name = "ชื่อตำแหน่งงาน")]    
       public string Name { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลรายละเอียด")]
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลลลำดับ")]
        [Display(Name = "ลำดับ")]
        public int Order { get; set; }

    }
}
