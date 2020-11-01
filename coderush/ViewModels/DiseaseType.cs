using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vds.Models;

namespace vds.ViewModels
{
    public class DiseaseTypeView
    {
        public DiseaseType DiseaseType { get; set; }
        public List<DiseaseType> DiseaseTypeList { get; set; }
    }
}
