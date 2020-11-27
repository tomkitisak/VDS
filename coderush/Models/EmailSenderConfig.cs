using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    //type of asset
    public class EmailSenderConfig : Base
    {
        public string Id { get; set; }

       
        [Required(ErrorMessage = "กรุณากรอกข้อมูล Mail Server")]
        [Display(Name = "Mail Server")]     
        public string MailServer { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูล Mail Port")]
        [Display(Name = "Port")]
        public int MailPort { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลชื่อผู้ส่ง")]
        [Display(Name = "ชื่อผู้ส่งอีเมล์")]
        public string SenderName { get; set; }

        [EmailAddress(ErrorMessage = "กรอกข้อมูลรูปแบบอีเมล์ไม่ถูกต้อง")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูลอีเมล์")]
        [Display(Name = "อีเมล์")]
        public string Email { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลรหัสผ่าน")]
        [StringLength(100, ErrorMessage = "รหัสผ่านไม่น้อยกว่า {2} ตัวอักษร และไม่เกิน {1} ตัวอักษร", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสผ่าน")]
        public string Password { get; set; }

    }
}
 