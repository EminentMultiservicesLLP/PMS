using PMS.Areas.Review.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Areas.Reports.Models
{
    public class ReviewReportsModel
    {
        public int EmpId { get; set; }
        public int EmpReviewId { get; set; }
        public string EmpName { get; set; }
        public string DesignationName { get; set; }
        public string StrJoiningDate { get; set; }
        public string StrConfirmationDate { get; set; }
        public string StrLastPromoDate { get; set; }
        public string EmpSalary { get; set; }
        public string DeptName  { get; set; }
        public string AppraiserName { get; set; }
        public string AppraiserDesigName { get; set; }
        public int GradePoints { get; set; }
        public List<ReviewReportsModel> EmpRvwData { get; set; }
        public int DeptId { get; set; }
        public OutcomeEnum OutcomeValue { get; set; }
        public string OutComeManView { get; set; }
        public RecommendationsEnum RecommendationValue { get; set; }
        public string TrainingNeeds { get; set; }
        public int RecDesignationId { get; set; }
        public string RecSalary { get; set; }
        public string RecIncrment { get; set; }
        public string RecManView { get; set; }
        public string RecDesignationName { get; set; }
        public string Comments { get; set; }
        public string OutletName { get; set; }
        public string GradeRemark { get; set; } 
        public int RvwType { get; set; }      
        public bool IsShwMark { get; set; }
    }

    public class ReviewReportHeads
    {
        public int HeadId { get; set; }
        public int EmpReviewId { get; set; }
        public string HeadName { get; set; }        
        public int SubHeadId { get; set; }
        public double PointGiven { get; set; }
        public string HeadsManView { get; set; }
        public List<ReviewReportHeads> EmpRvwHeads { get; set; }  
    }

}