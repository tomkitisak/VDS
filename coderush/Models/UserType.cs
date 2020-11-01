using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    //type of ticket
    public class UserType : Base
    {
        public string UserTypeId { get; set; }
        [Required]
        [Display(Name = "ประเภทผู้ใช้")]
        public string Name { get; set; }
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }
    }
}
