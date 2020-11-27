using System.ComponentModel.DataAnnotations.Schema;

namespace vds.Models
{
    public class Province : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 

        public string ProvinceId { get; set; }
        public string ProvinceThai { get; set; }
        public string ProvinceEng { get; set; }
        public string RegionId { get; set; }
        public Region Region { get; set; }

    }
}

