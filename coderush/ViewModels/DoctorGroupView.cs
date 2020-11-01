using System.Collections.Generic;
using vds.Models;

namespace vds.ViewModels
{
    public class DoctorGroupView
    {
        public DoctorGroup DoctorGroup { get; set; }
        public GroupCoordinator GroupCoordinator { get; set; }
        public List<Doctor> DoctorList { get; set; } 

    }
}
