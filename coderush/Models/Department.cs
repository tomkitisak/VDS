using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    //division of a larger organization into parts with specific responsibility
    public class Department : Base
    {
        public string DepartmentId { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลชื่อหน่วยงาน")]
        [Display(Name = "ชื่อหน่วยงาน")]
        public string Name { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลรายละเอียด")]
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลลำกับ")]
        [Display(Name = "ลำดับ")]
        public int Order { get; set; }
    }
}
