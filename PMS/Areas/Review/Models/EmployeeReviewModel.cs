using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Areas.Review.Models
{
    public class EmployeeReviewModel
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string StrJoiningDate { get; set; }
        public string StrConfirmationDate { get; set; }
        public string StrLastPromoDate { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string AppraiserName { get; set; }
        public string AppraiserDesigName { get; set; }
        public double Salary { get; set; }
        public int GradePoints { get; set; }
        public OutcomeEnum OutcomeValue { get; set; }
        public string GradeName { get; set; }
        public int DeptId { get; set; }

        public int EmpFinalReviewId { get; set; }
        public int EmpReviewId { get; set; }
        public int EmpId { get; set; }
        public int AppraiserEmpId { get; set; }      
        public string OutComeManView { get; set; }
        public int GradeId { get; set; }    
        public int AppraiserStatus { get; set; }
        public int AppraiserTwoEmpId { get; set; }
        public int AppraiserTwoStatus { get; set; }
        public List<EmpReviewHeads> HeadList { get; set; }
        public EmpRecommendations Recommendations { get; set; }    
        public EmpComponents Components { get; set; }
        public bool IsSubmit { get; set; }
        public string Comments { get; set; } 
        public string AppraiserTwoComments { get; set; }     
        public string strRecommendation { get; set; }
        public string strOutcome { get; set; }
        public string GradeRemark { get; set; } 
        public decimal GradeScore { get; set; }
        public string ApprOneStatus { get; set; }
        public string ApprTwoStatus { get; set; }
        public bool IsShwBtn { get; set; }
        public int Year  { get; set; }
        public int AppraiserDesgId { get; set; }
        public int QuestionnaireId { get; set; }


        public int? InsertedBy { get; set; }
        public DateTime? InsertedOn { get; set; }
        public string InsertedMacName { get; set; }
        public string InsertedMacId { get; set; }
        public string InsertedIpAddress { get; set; }

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedMacName { get; set; }
        public string UpdatedMacID { get; set; }
        public string UpdatedIPAddress { get; set; }
    }

   
    public class EmpReviewHeads
    {
        public int EmpFinalReviewId { get; set; }
        public int EmpReviewId { get; set; }
        public int SubHeadId { get; set; }
        public double PointGiven { get; set; }
        public string HeadsManView { get; set; }
        public string HeadsRevView { get; set; }
    }

    public class EmpRecommendations
    {
        public int EmpReviewId { get; set; }       
        public RecommendationsEnum RecommendationValue { get; set; }
        public string TrainingNeeds { get; set; }
        public int RecDesignationId { get; set; }
        public double RecSalary { get; set; }
        public double RecIncrment { get; set; }
        public string RecManView { get; set; }
        public EmpRecommendations() {}
        public EmpRecommendations(RecommendationsEnum _recommendationValue,string _tN,int _desgnId,double _sal,double _incr,string _recManView ="") //bool correctVal 
        {
            RecommendationValue = _recommendationValue;
            TrainingNeeds = _tN;
            RecDesignationId = _desgnId;
            RecSalary = _sal;
            RecIncrment = _incr;
            RecManView = _recManView;
        }

    }


    public class EmpComponents
    {
        public int EmpRvwComponentId { get; set; }
        public int EmpReviewId { get; set; }
        public int EmpId { get; set; }
        public double PreviousGross { get; set; }
        public double PreviousIncrement { get; set; }
        public DateTime PreviousDate { get; set; }
        public string StrPreviousDate { get; set; }
        public double LastGross { get; set; }
        public double Increment { get; set; }
        public double NewGross { get; set; }
        public DateTime IncrementDate { get; set; }
        public int LastDesgId { get; set; }
        public int NewDesgID { get; set; }
        public int LastDeptId { get; set; }
        public EmpComponents() { }
        public EmpComponents(double _previousGross, double _previousIncrement)
        {
            PreviousGross = _previousGross;
            PreviousIncrement = _previousIncrement;
        }

    }






    public enum Status {
        Save = 1,
        Submit = 2
    }

    public enum OutcomeEnum
    {
        Below = 1,
        Meets = 2,
        Exceeds = 3
    }

    public enum RecommendationsEnum
    {
        Training = 1,
        Promotion = 2,
        OnlyIncrement = 3 
    }

}