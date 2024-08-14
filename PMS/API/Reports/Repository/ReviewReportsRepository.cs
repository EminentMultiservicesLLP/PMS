using PMS.API.Reports.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Areas.Reports.Models;
using CommonDataLayer.DataAccess;
using System.Data;
using PMS.QueryCollection.Reports;
using PMS.Areas.Review.Models;
using PMS.Areas.Masters.Models;

namespace PMS.API.Reports.Repository
{
    public class ReviewReportsRepository : IReviewReports
    {
        public List<ReviewReportsModel> GetReviewReport(int rvwtype, int outletId, int gradeId, int desgId, int empId,  int Year)
        {
            int UserId = Convert.ToInt32(HttpContext.Current.Session["AppUserId"].ToString());
        
            List<ReviewReportsModel> list = null;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
                paramCollection.Add(new DBParameter("rvwtype", rvwtype, DbType.Int32));
                paramCollection.Add(new DBParameter("outletId", outletId, DbType.Int32));
                paramCollection.Add(new DBParameter("gradeId", gradeId, DbType.Int32));
                paramCollection.Add(new DBParameter("desgId", desgId, DbType.Int32));
                paramCollection.Add(new DBParameter("empId", empId, DbType.Int32));
                paramCollection.Add(new DBParameter("Year", Year, DbType.Int32));
                
                //DataTable dtReviewMst = dbHelper.ExecuteDataTable(ReportsQueries.GetReviewReport,paramCollection, CommandType.StoredProcedure);
                DataTable dtReviewMst = dbHelper.ExecuteDataTableWithTimeout(ReportsQueries.GetReviewReport, paramCollection, CommandType.StoredProcedure,0);
                list = dtReviewMst.AsEnumerable()
                            .Select(row => new ReviewReportsModel
                            {
                                EmpName = row.Field<string>("EmpName"),
                                EmpReviewId = row.Field<int>("EmpReviewId"),
                                DesignationName = row.Field<string>("DesignationName"),
                                StrJoiningDate = row.Field<string>("JoiningDate"),
                                StrConfirmationDate = row.Field<string>("ConfirmationDate"),
                                StrLastPromoDate = row.Field<string>("LastPromoDate"),
                                EmpSalary = Convert.ToString(row.Field<double>("EmpSalary")),
                                AppraiserName = row.Field<string>("AppraiserName"),
                                AppraiserDesigName = row.Field<string>("AppraiserDesigName"),
                                GradePoints = row.Field<int>("GradePoints"),
                                DeptId = row.Field<int>("DeptId"),
                                OutcomeValue = (OutcomeEnum)row.Field<int>("OutcomeValue"),
                                OutComeManView = row.Field<string>("OutComeManView"),
                                RecommendationValue = (RecommendationsEnum)row.Field<int>("RecommendationValue"),
                                TrainingNeeds = row.Field<string>("TrainingNeeds"),
                                RecDesignationId = row.Field<int>("RecDesignationId"),
                                RecSalary = Convert.ToString(row.Field<double>("RecSalary")),
                                RecIncrment = Convert.ToString(row.Field<double>("RecIncrment")),
                                RecManView = row.Field<string>("RecManView"),
                                RecDesignationName = row.Field<string>("RecDesignationName"),
                                Comments = row.Field<string>("Comments"),
                                DeptName = row.Field<string>("DeptName"),
                                OutletName = row.Field<string>("OutletName"),
                                GradeRemark = row.Field<string>("GradeRemark"),
                                RvwType = rvwtype,
                                IsShwMark = row.Field<bool>("IsShwMark")                               
                            }).ToList();
            }
            return list;
        }


        public List<ReviewReportHeads> GetReportHeads( int EmpReviewId,int RvwType)
        {
            List<ReviewReportHeads> AllHeads = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            //paramCollection.Add(new DBParameter("DeptId", DeptId, DbType.Int32));
            paramCollection.Add(new DBParameter("EmpReviewId", EmpReviewId, DbType.Int32));
            paramCollection.Add(new DBParameter("RvwType", RvwType, DbType.Int32)); 
            using (DBHelper dbHelper = new DBHelper())
            {
                DataSet dsSubheads = dbHelper.ExecuteDataSet(ReportsQueries.GetReportHeads, paramCollection, CommandType.StoredProcedure);
                AllHeads = dsSubheads.Tables[0].AsEnumerable()
                   .Select(row => new ReviewReportHeads
                   {
                       SubHeadId = row.Field<int>("SubHeadId"),
                       HeadName = row.Field<string>("HeadName"),
                       HeadId = row.Field<int>("HeadId"),
                       PointGiven = row.Field<double>("PointGiven"),
                       HeadsManView = row.Field<string>("HeadsManView")
                   }).ToList();

            }

            return AllHeads;
        }

        public List<EmployeeModel> GetAllEmployee()
        {
            int UserId = Convert.ToInt32(HttpContext.Current.Session["AppUserId"].ToString());
            List<EmployeeModel> Employees = null;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
                DataTable dtTable = dbHelper.ExecuteDataTable(ReportsQueries.GetAllEmployee, paramCollection, CommandType.StoredProcedure);
                Employees = dtTable.AsEnumerable()
                   .Select(row => new EmployeeModel
                   {
                       EmpId = row.Field<int>("EmpId"),
                       EmpName = row.Field<string>("EmpName")
                   }).ToList();

                return Employees;
            }
        }

        public List<FinalReviewedReportModel> GetFinalRvwdRpt(int outletId, int gradeId, int desgId, int empId, int Year)
        {
           

            List<FinalReviewedReportModel> list = null;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();                        
                paramCollection.Add(new DBParameter("outletId", outletId, DbType.Int32));
                paramCollection.Add(new DBParameter("gradeId", gradeId, DbType.Int32));
                paramCollection.Add(new DBParameter("desgId", desgId, DbType.Int32));              
                paramCollection.Add(new DBParameter("Year", Year, DbType.Int32));
                DataTable dtReviewMst = dbHelper.ExecuteDataTable(ReportsQueries.GetFinalRvwdRpt, paramCollection, CommandType.StoredProcedure);
                list = dtReviewMst.AsEnumerable()
                            .Select(row => new FinalReviewedReportModel
                            {
                                EmpCode = row.Field<string>("EmpCode"),
                                EmpName = row.Field<string>("EmpName"),
                                JoiningDate = row.Field<string>("JoiningDate"),
                                DeptName = row.Field<string>("DeptName"),
                                DesignationName = row.Field<string>("DesignationName"),
                                OutletName = row.Field<string>("OutletName"),
                                CC1 = row.Field<string>("Cc1"),
                                CC2 = row.Field<string>("Cc2"),
                                CC3 = row.Field<string>("Cc3"),
                                Eligibility = row.Field<string>("Eligibility"),
                                PreviousIncrement = row.Field<double>("PreviousIncrement"),
                                PreviousGross = row.Field<double>("PreviousGross"),
                                LastIncrMnth = row.Field<string>("LastIncrMnth"),
                                LastGross = row.Field<double>("LastGross"),
                                KRA = row.Field<decimal>("KRA"),
                                Rating = row.Field<decimal>("Rating"),
                                IncrPercentage = row.Field<double>("IncrPercentage"),
                                Increment = row.Field<double>("Increment"),
                                NewGross = row.Field<double>("NewGross"),
                                IsPromotion = row.Field<string>("IsPromotion"),
                                PromotionDesg = row.Field<string>("PromotionDesg")                               
                            }).ToList();

            }

            return list;
        }


        public List<StatusReportModel> GetStatusRpt(int outletId, int gradeId, int desgId, int Year)
        {
            List<StatusReportModel> list = null;
            int UserId = Convert.ToInt32(HttpContext.Current.Session["AppUserId"].ToString());
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
                paramCollection.Add(new DBParameter("outletId", outletId, DbType.Int32));
                paramCollection.Add(new DBParameter("gradeId", gradeId, DbType.Int32));
                paramCollection.Add(new DBParameter("desgId", desgId, DbType.Int32));
                paramCollection.Add(new DBParameter("Year", Year, DbType.Int32));
                DataTable dtReviewMst = dbHelper.ExecuteDataTable(ReportsQueries.GetStatusRpt, paramCollection, CommandType.StoredProcedure);
                list = dtReviewMst.AsEnumerable()
                            .Select(row => new StatusReportModel
                            {
                                EmpCode = row.Field<string>("EmpCode"),
                                EmpName = row.Field<string>("EmpName"),
                                OutletName = row.Field<string>("OutletName"),
                                AppraiserOneName = row.Field<string>("AppraiserOneName"),
                                AppraiserTwoName = row.Field<string>("AppraiserTwoName"),
                                AppraiserOneStatus = row.Field<string>("AppraiserOneStatus"),
                                AppraiserTwoStatus = row.Field<string>("AppraiserTwoStatus")
                            }).ToList();
            }
            return list;
        }

    }
}