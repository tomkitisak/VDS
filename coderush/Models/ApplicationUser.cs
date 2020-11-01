using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vds.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        //override identity user, add new column
        public bool isSuperAdmin { get; set; } = false;
        public String UserTypeId  { get; set; }
       

    }
}
