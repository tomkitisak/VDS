using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    public class Hospital : Base
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


        [Display(Name = "พิกัด LAT")]
        public string Lat { get; set; }
        [Display(Name = "พิกัด LONG")]
        public string Long { get; set; }




    }
}
