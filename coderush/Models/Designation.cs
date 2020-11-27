using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vds.Models
{
    // official position of an employee
    public class Designation : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public string DesignationId { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลชื่อตำแหน่งงาน")]
        [Display(Name = "ชื่อตำแหน่งงาน")]    
       public string Name { get; set; }
        
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }
        
        [Display(Name = "ลำดับ")]
        public int Order { get; set; }

    }
}
