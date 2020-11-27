using System.ComponentModel.DataAnnotations.Schema;

namespace vds.Models
{
    public class Region : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 

        public string RegionId { get; set; }

        public string RegionName { get; set; }


    }
}
