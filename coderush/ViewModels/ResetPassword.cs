using System.ComponentModel.DataAnnotations;

namespace vds.ViewModels
{
    //view model for reset password
    public class ResetPassword
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "รหัสผ่านต้องมีความยาวไม่น้อยกว่า 6 ตัวอักษร!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสผ่านใหม่")]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "รหัสผ่านต้องมีความยาวไม่น้อยกว่า 6 ตัวอักษร!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "ยืนยันรหัสผ่าน")]
        [Compare("NewPassword", ErrorMessage = "รหัสผ่านไม่ตรงกัน!")]
        public string ConfirmPassword { get; set; }
    }
}
