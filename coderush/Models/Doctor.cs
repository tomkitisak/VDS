using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    public class Doctor : Base
    {
        public string DoctorId { get; set; }

        //basic info
        [Required(ErrorMessage = "กรุณากรอกคำนำหน้าชื่อ")]
        [Display(Name = "คำนำหน้าชื่อ")]
        public string PrefixTypeId { get; set; }
        public PrefixType PrefixType { get; set; }


        [Required]
        [Display(Name = "ชื่อ")]
        public string FirstName { get; set; }
        [Display(Name = "นามสกุล")]
        public string LastName { get; set; }

        [Display(Name = "ชื่อ-สกุล")]
        public string FullName
        {
            get { return   FirstName + " " + LastName; }
        }

        [EmailAddress(ErrorMessage ="กรอกข้อมูลรูปแบบอีเมล์ไม่ถูกต้อง") ]
        [Required(ErrorMessage = "กรุณากรอกข้อมูลอีเมล์")]
        [Display(Name = "อีเมล์")]
        public string Email { get; set; }


        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "กรอกข้อมูลรูปแบบหมายเลขโทรศัพท์ไม่ถูกต้อง")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูลหมายเลขโทรศัพท์")]
        [Display(Name = "หมายเลขโทรศัพท์")]
        public string Phone { get; set; }



        [Display(Name = "ไลน์ไอดี")]

        public string LineId { get; set; }


        [Required(ErrorMessage = "กรุณากรอกข้อมูลที่อยู่")]
        [Display(Name = "ที่อยู่ 1")]
        public string Address1 { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลตำบล")]
        [Display(Name = "ตำบล / แขวง")]
        public string SubDistrict { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลอำเภอ")]
        [Display(Name = "อำเภอ / เขต")]
        public string District { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลจังหวัด")]
        [Display(Name = "จังหวัด")]
        public string Province { get; set; }

        [Display(Name = "รหัสไปรษณีย์")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลใบประกอบวิชาชีพ")]
        [Display(Name = "ใบประกอบวิชาชีพแพทย์")]
        public string MDLicense { get; set; }

        [Display(Name = "ภาพโปรไฟล์")]
        public byte[] ImageData { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูลสาขาความเชี่ยวชาญ")]
        [Display(Name = "สาขาที่เชียวชาญ")]
        public string DoctorTypeId { get; set; }
        public DoctorType DoctorType { get; set; }

    }
}
