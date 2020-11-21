using Microsoft.EntityFrameworkCore;

namespace vds.ViewModels
{
    [Keyless]
    public class HospitalViewRegionId
    {

        public string HospitalId { get; set; }
        public string RegionId { get; set; }
    }
}
