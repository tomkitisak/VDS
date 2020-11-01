using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using vds.Models;

namespace vds.ViewModels
{
    public class PatientSelectViewModel
    {

        public string PatientId { get; set; }

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
        [Required]
        public string Gender { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลหมายเลขโทรศัพท์")]
        [Display(Name = "หมายเลขโทรศัพท์")]

        public string Phone { get; set; }
      

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

        public SelectListItem PatientSelect { get; set; }

    }
}
