using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vds.Models;

namespace vds.ViewModels
{
    public class DepartmentView
    {
        public Department Department { get; set; }
        public List<Department> DepartmentList { get; set; }
    }
}
