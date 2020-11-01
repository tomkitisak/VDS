using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vds.Models;

namespace vds.ViewModels
{
    public class PrefixTypeView
    {
        public PrefixType PrefixType { get; set; }
        public List<PrefixType> PrefixTypeList { get; set; }
    }
}
