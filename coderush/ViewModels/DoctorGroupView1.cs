using vds.Models;

namespace vds.ViewModels
{
    public class DoctorGroupView1
    {
        public string DoctorGroupId { get; set; }

        public string DoctorGroupName { get; set; }


        public string DoctorTypeId { get; set; }
        public DoctorType DoctorType { get; set; }

        public string PrefixTypeId { get; set; }
        public PrefixType PrefixType { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNo { get; set; }

        public byte[] ImageData { get; set; }

    }
}
