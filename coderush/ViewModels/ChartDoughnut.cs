using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vds.ViewModels
{
    public class ChartDoughnut
    {
        public string[] Labels { get; set; }
        public int[] Data { get; set; }
        public string[] Colors { get; set; }
    }
}
