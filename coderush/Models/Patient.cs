using System;
using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    public class Patient : Base
    {
        public string PatientId { get; set; }

        //basic info
        //basic info
        [Required(ErrorMessage = "กรุณากรอกคำนำหน้าชื่อ")]
        [Display(Name = "คำนำหน้าชื่อ")]
        public string PrefixTypeId { get; set; }
        public PrefixType PrefixType { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลชื่อ")]
        [Display(Name = "ชื่อ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลนามสกุล")]
        [Display(Name = "นามสกุล")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลเพศ")]
        [Display(Name = "เพศ")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลวัน-เดือน-ปี-เกิด")]
        [Display(Name = "วัน-เดือน-ปีเกิด")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; } 

        public int  Age  
        {
            get
            {
                var age = DateTime.Today.Year - this.DateOfBirth.Year;
                var month = DateTime.Today.Month - this.DateOfBirth.Month;
                if (month < 0 || (month == 0 && DateTime.Today.Date < this.DateOfBirth.Date))
                {
                    age = age - 1;
                }
                return age;
            }

        }     
    [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "กรอกข้อมูลรูปแบบหมายเลขโทรศัพท์ไม่ถูกต้อง")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูลหมายเลขโทรศัพท์")]
        [Display(Name = "หมายเลขโทรศัพท์")]
        public string Phone { get; set; }

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

        [Required(ErrorMessage = "กรุณากรอกข้อมูลรหัสไปรษณีย์")]
        [Display(Name = "รหัสไปรษณีย์")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลปัญหา")]
        [Display(Name = "ปัญหา")]
        public string Problem { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลโรค")]
        [Display(Name = "โรค")]
        public string DiseaseTypeId { get; set; }

        public DiseaseType DiseaseType { get; set; }


        [Required(ErrorMessage = "กรุณากรอกข้อมูลโรงพยาบาล")]
        [Display(Name = "โรงพยาบาล")]
        public string HospitalId { get; set; }
        public Hospital Hospital { get; set; }

        public int Status { get; set; }


    }
}
