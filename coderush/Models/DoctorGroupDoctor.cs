using System.ComponentModel.DataAnnotations;

namespace vds.Models
{
    public class DoctorGroupDoctor : Base
    {
        public string DoctorGroupDoctorId { get; set; }
        public string DoctorGroupId { get; set; }
        public DoctorGroup DoctorGroup { get; set; }
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

     

    }
}
