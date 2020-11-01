using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using vds.Models;

namespace vds.ViewModels
{

    public class DoctorSelectedViewModel
    {
        
            public string DoctorGroupId { get; set; }
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


            [Display(Name = "อีเมล์")]
            public string Email { get; set; }

            [Required(ErrorMessage = "กรุณากรอกข้อมูลหมายเลขโทรศัพท์")]
            [Display(Name = "หมายเลขโทรศัพท์")]

            public string Phone { get; set; }



            [Display(Name = "ไลน์ไอดี")]
            public string LineId { get; set; }


            [Display(Name = "ภาพโปรไฟล์")]
            public byte[] ImageData { get; set; }

            [Display(Name = "สาขาที่เชียวชาญ")]
            public string DoctorTypeId { get; set; }
            public DoctorType DoctorType { get; set; }

            public SelectListItem DoctorSelect { get; set; }

        

    }
   
}
