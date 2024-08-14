using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Areas.Reports.Models
{
    public class FinalReviewedReportModel
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string JoiningDate { get; set; }
        public string DeptName { get; set; }
        public string DesignationName { get; set; }
        public string OutletName { get; set; }
        public string CC1 { get; set; }
        public string CC2 { get; set; }
        public string CC3 { get; set; }
        public string  Eligibility { get; set; }
        public double PreviousIncrement { get; set; }
        public double PreviousGross { get; set; }
        public string LastIncrMnth { get; set; }
        public double LastGross { get; set; }
        public decimal KRA { get; set; }
        public decimal Rating { get; set; }
        public double IncrPercentage { get; set; }
        public double Increment { get; set; }
        public double NewGross { get; set; }  
        public string IsPromotion { get; set; }
        public string PromotionDesg { get; set; }
        public List<FinalReviewedReportModel> FinalRvwdData { get; set; }
    }
}