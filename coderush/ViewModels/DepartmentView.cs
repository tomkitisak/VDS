using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vds.Models;

namespace vds.ViewModels
{
    public class DoctorTypeView

    {
        public DoctorType DoctorType { get; set; }
        public List<DoctorType> DoctorTypeList { get; set; }
    }
}
