using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using vds.Models;

namespace vds.Services.CustomValidation
{
    public class DateNotLessthan : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var job = (Job)validationContext.ObjectInstance;

            if (job.StartDate == null)
                return new ValidationResult("required.");

            DateTime startdate = job.StartDate;
            DateTime enddate = job.EndDate;
            int result = DateTime.Compare(startdate, enddate);

            return (result <= 0) ? ValidationResult.Success : new ValidationResult("วันเวลาเริ่มต้น มากกว่า วันเวลาสิ้นสุด!");
        }

    }
}
