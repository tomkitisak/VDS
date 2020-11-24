using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vds.Services.CustomValidation;

namespace vds.Models
{
    //type of allowance
    public class Job : Base
    {
        public string JobId { get; set; }
        [Required(ErrorMessage = "กรุณากรอกชื่องาน!")]
        [Display(Name = "ชื่องาน")]
        public string Name { get; set; }

        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "วันที่เปิดงาน")]
        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "กรุณากรอกวันโพสต์!")]
        [Display(Name = "วันที่โพสต์")]
        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; } = DateTime.Today;

        [Display(Name = "โพสต์")]
        public Boolean IsPosted { get; set; }

        [Display(Name = "วันบันทึกนัด")]
        [DataType(DataType.Date)]
        public DateTime AppEntryDate { get; set; } = DateTime.Today;

        [Display(Name = "นัด")]
        public Boolean IsAppointed { get; set; }

        [Required(ErrorMessage ="กรุณากรอกวันเวลาเริ่มต้น!")]  
        [Display(Name = "วันนัด-เวลาเริ่มต้น")]
        [DataType(DataType.DateTime)]
        public DateTime AppStartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "กรุณากรอกวันเวลาสิ้นสุด!")]
        [Display(Name = "วันนัด-เวลาสิ้นสุด")]
        [DataType(DataType.DateTime)]
        
        public DateTime AppEndDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "กรุณากรอกวันจบงาน!")]
        [Display(Name = "วันปิดงาน")]
        [DataType(DataType.Date)]
        public DateTime JobEndEntryDate { get; set; } = DateTime.Today;

        [Display(Name = "ปิด")]
        public Boolean IsDone { get; set; }

        [Required(ErrorMessage = "กรุณากรอกวันเวลาเริ่มต้น!")]
        [Display(Name = "วัน-เวลาเริ่มต้น")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "กรุณากรอกวันเวลาสิ้นสุด!")]
        [Display(Name = "วัน-เวลาสิ้นสุด")]
        [DataType(DataType.DateTime)]    
        public DateTime EndDate { get; set; } = DateTime.Today;

        [Display(Name = "หมายเหตุประกอบนัด")]
        public String Remark1 { get; set; }


        [Display(Name = "ผลการดำเนินการ")]
        public String Remark2 { get; set; }

        [Display(Name = "จำนวนผู้ป่วย(คน)")]
        public int TotalPatients { get; set; }

        [Display(Name = "จำนวนพทย์(คน)")]
        public int TotalDoctors { get; set; }

        [Display(Name = "สถานะ")]
        public string JobStatusId { get; set; }
        public JobStatus JobStatus { get; set; }

        [Required]
        [Display(Name = "โรงพยาบาล")]
        public string HospitalId { get; set; }
        public Hospital Hospital { get; set; }
    }
   

}
