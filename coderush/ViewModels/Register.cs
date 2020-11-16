using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using vds.Models;

namespace vds.ViewModels
{
    //view model for register screen
    public class Register
    {
         
        [EmailAddress(ErrorMessage = "กรอกข้อมูลรูปแบบอีเมล์ไม่ถูกต้อง")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูลอีเมล์")]
        [Display(Name = "อีเมล์")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "กรอกข้อมูลรูปแบบหมายเลขโทรศัพท์ไม่ถูกต้อง")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูลหมายเลขโทรศัพท์")]
        [Display(Name = "หมายเลขโทรศัพท์")]
        public string PhoneNumber { get; set; }
 
        [Display(Name = "ยืนยันอีเมล์")]
        public bool EmailConfirmed { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลรหัสผ่าน")]
        [StringLength(100, ErrorMessage = "รหัสผ่านไม่น้อยกว่า {2} ตัวอักษร และไม่เกิน {1} ตัวอักษร", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสผ่าน")]
        public string Password { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลยืนยันรหัสผ่าน")]
        [DataType(DataType.Password)]
        [Display(Name = "ยืนยันรหัสผ่าน")]
        [Compare("Password", ErrorMessage = "รหัสผ่าน 2 ครั้งไม่ตรงกัน !")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "ประเภทผู้ใช้")]
        [Required(ErrorMessage = "กรุณาเลือกประเภทผู้ใช้")]
        public string UserTypeId { get; set; }

        public UserType UserType { get; set; }

    }
}
