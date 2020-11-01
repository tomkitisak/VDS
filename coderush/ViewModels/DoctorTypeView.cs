using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vds.Models;

namespace vds.ViewModels
{
    public class DesignationView
    {
        public Designation Designation { get; set; }
        public List<Designation> DesignationList { get; set; }
    }
}
