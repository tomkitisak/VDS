using System.ComponentModel.DataAnnotations;
using vds.Models;

namespace vds.ViewModels
{

    public class HospitalView
    {

        public string HospitalId { get; set; }

        //basic info
        [Required(ErrorMessage = "กรุณากรอกข้อมูลชื่อโรงพยาบาล")]
        [Display(Name = "ชื่อโรงพยาบาล")]
        public string HospitalName { get; set; }


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


        [Display(Name = "จำนวนเตียง")]
        public int Size { get; set; }


        [Display(Name = "จำนวนห้องผ่าตัด")]
        public int OperatingRoom { get; set; }

        [Display(Name = "บริการศักยภาพ")]
        public string Service { get; set; }

        public string Lat { get; set; }
        public string Long { get; set; }

        // Director
        [Display(Name = "คำนำหน้าชื่อ")]
        public string DPrefixTypeId { get; set; }
        public PrefixType DPrefixType { get; set; }

        [Required]
        [Display(Name = "ชื่อ")]
        public string DFirstName { get; set; }
        [Display(Name = "นามสกุล")]
        public string DLastName { get; set; }

        [Display(Name = "ไลน์ไอดี")]
        public string DLineId { get; set; }


        [Display(Name = "อีเมล์")]
        public string DEmail { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลหมายเลขโทรศัพท์")]
        [Display(Name = "หมายเลขโทรศัพท์")]

        public string DPhone { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลที่อยู่")]
        [Display(Name = "ที่อยู่")]
        public string DAddress1 { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลตำบล")]
        [Display(Name = "ตำบล / แขวง")]
        public string DSubDistrict { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลอำเภอ")]
        [Display(Name = "อำเภอ / เขต")]
        public string DDistrict { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลจังหวัด")]
        [Display(Name = "จังหวัด")]
        public string DProvince { get; set; }

        [Display(Name = "รหัสไปรษณีย์")]
        public string DZipCode { get; set; }


        // Coordinator
        [Display(Name = "คำนำหน้าชื่อ")]
        public string CPrefixTypeId { get; set; }
        public PrefixType CPrefixType { get; set; }
        [Required]
        [Display(Name = "ชื่อ")]
        public string CFirstName { get; set; }
        [Display(Name = "นามสกุล")]
        public string CLastName { get; set; }

        [Display(Name = "ไลน์ไอดี")]
        public string CLineId { get; set; }


        [Display(Name = "อีเมล์")]
        public string CEmail { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลหมายเลขโทรศัพท์")]
        [Display(Name = "หมายเลขโทรศัพท์")]

        public string CPhone { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลที่อยู่")]
        [Display(Name = "ที่อยู่")]
        public string CAddress1 { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลตำบล")]
        [Display(Name = "ตำบล / แขวง")]
        public string CSubDistrict { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลอำเภอ")]
        [Display(Name = "อำเภอ / เขต")]
        public string CDistrict { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลจังหวัด")]
        [Display(Name = "จังหวัด")]
        public string CProvince { get; set; }

        [Display(Name = "รหัสไปรษณีย์")]
        public string CZipCode { get; set; }


        [Required]
        [Display(Name = "Designation")]
        public string DesignationId { get; set; }

        public Designation Designation { get; set; }
        [Required]
        [Display(Name = "Department")]
        public string DepartmentId { get; set; }
        public Department Department { get; set; }



    }
}
