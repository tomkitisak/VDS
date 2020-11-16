using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using vds.Models;

namespace vds.ViewModels
{
    //view model for register screen
    public class UserView
    {
        public string Id { get; set; }

        [Display(Name = "ชื่อผู้ใช้")]
        public string UserName { get; set; }


        [Required]
        [EmailAddress]
        [Display(Name = "อีเมล์")]
        public string Email { get; set; }

        [EmailAddress]
        [Display(Name = "ยืนยันอีเมล์")]
        public bool EmailConfirmed { get; set; }

        [Phone]
        [Display(Name = "หมายเลขโทรศัพท์")]     
        public string PhoneNumber { get; set; }

        public bool isSuperAdmin { get; set; }

        [Display(Name = "ประเภทผู้ใช้")]
        public string UserTypeId { get; set; }

        [Display(Name = "ระดับผู้ใช้")]
        public int UserLevel { get; set; }

        [Display(Name = "กลุ่มผู้ใช้")]
        public string Name { get; set; }

     

    }
}

 