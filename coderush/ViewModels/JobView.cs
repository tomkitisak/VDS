using System.Collections.Generic;
using vds.Models;

namespace vds.ViewModels
{
    public class JobView
    {
        public Job Job { get; set; }
        public List<Patient> PatientList { get; set; }
        public List<Doctor> DoctorList { get; set; } 

    }
}
