using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Areas.Reports.Models
{
    public class StatusReportModel
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }      
        public string OutletName { get; set; }
        public string AppraiserOneName { get; set; }
        public string AppraiserTwoName { get; set; }
        public string AppraiserOneStatus { get; set; }
        public string AppraiserTwoStatus { get; set; }
        public List<StatusReportModel> EmpRvwStatus { get; set; }

    }
}