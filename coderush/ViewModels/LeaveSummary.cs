﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vds.ViewModels
{
    public class LeaveSummary
    {
        public string Approved { get; set; }
        public string ApprovedPercentage { get; set; }
        public string NotApproved { get; set; }
        public string NotApprovedPercentage { get; set; }
        public string PaidLeave { get; set; }
        public string PaidLeavePercentage { get; set; }
        public string NotPaidLeave { get; set; }
        public string NotPaidLeavePercentage { get; set; }
    }
}
