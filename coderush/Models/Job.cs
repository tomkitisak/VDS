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
        [Required]
        [Display(Name = "ชื่องาน")]
        public string Name { get; set; }
        [Display(Name = "รายละเอียด")]
        public string Description { get; set; }


        [Required]
        [Display(Name = "วันที่เปิดงาน")]
        [DataType(DataType.Date)]
        public DateTime TransDate { get; set; } = DateTime.Now;


        [Required]
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

        [Required]
        [DateLessThan("AppEndDate", ErrorMessage = "Not valid")]
        [Display(Name = "วันนัด-เวลาเริ่มต้น")]
        [DataType(DataType.DateTime)]
        public DateTime AppStartDate { get; set; } = DateTime.Today;
        [Required]
        [Display(Name = "วันนัด-เวลาสิ้นสุด")]
        [DataType(DataType.DateTime)]
        public DateTime AppEndDate { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "วันปิดงาน")]
        [DataType(DataType.Date)]
        public DateTime JobEndEntryDate { get; set; } = DateTime.Today;

        [Display(Name = "ปิด")]
        public Boolean IsDone { get; set; }

        [Required]
        [Display(Name = "วัน-เวลาเริ่มต้น")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "วัน-เวลาสิ้นสุด")]
        [DataType(DataType.DateTime)]
        [DateNotLessthan]
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
    public class DateLessThanAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _comparisonProperty;

        public DateLessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (currentValue > comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-error", error);
        }
    }


}
