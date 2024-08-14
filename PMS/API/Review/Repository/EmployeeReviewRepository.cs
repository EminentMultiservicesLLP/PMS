using PMS.API.Review.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Areas.Masters.Models;
using CommonDataLayer.DataAccess;
using System.Data;
using PMS.QueryCollection.Review;
using CommonLayer;
using PMS.Areas.Review.Models;
using CommonLayer.Extensions;

namespace PMS.API.Review.Repository
{ 
    public class EmployeeReviewRepository : IEmployeeReview
    {
        private static readonly ILogger _logger = Logger.Register(typeof(EmployeeReviewRepository));


        public List<SubheadsModel> GetHeads(int QuestionnaireId, int EmpReviewId)
        {
            List<SubheadsModel> AllHeads = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("QuestionnaireId", QuestionnaireId, DbType.Int32));
            paramCollection.Add(new DBParameter("EmpReviewId", EmpReviewId, DbType.Int32));
            //List<ReviewPoint> Reviews = new List<ReviewPoint>();
            using (DBHelper dbHelper = new DBHelper())
            {
                DataSet dsSubheads  = dbHelper.ExecuteDataSet(ReviewQueries.GetHeads, paramCollection, CommandType.StoredProcedure);
                AllHeads = dsSubheads.Tables[0].AsEnumerable()
                   .Select(row => new SubheadsModel
                   {
                       SubHeadId = row.Field<int>("SubHeadId"),
                       HeadName = row.Field<string>("HeadName"),
                       HeadId = row.Field<int>("HeadId"),
                       PointGiven = row.Field<double>("PointGiven"),
                       HeadsManView =  row.Field<string>("HeadsManView"),
                       HeadsRevView = row.Field<string>("HeadsRevView")
                   }).ToList();
            
                AllHeads[0].ReviewPoints = dsSubheads.Tables[1].AsEnumerable()
                   .Select(row => new ReviewPoint
                   {
                     Point = row.Field<double>("Point"),
                     Name = row.Field<string>("ReviewName")
                   }).ToList();
            }

            return AllHeads;
        }
        

        public int CreateReview(EmployeeReviewModel model, DBHelper dbHelper)
        {
            int tempateResult = 0;
            TryCatch.Run(() =>
            {
                var EmpReview = CreateReviewMst(model, dbHelper);
                CreateReviewDtls(EmpReview, dbHelper);
                tempateResult = 1;

            }).IfNotNull(ex =>
            {
                _logger.LogError("error at CreateReview" + ex.Message + Environment.NewLine + ex.StackTrace);
            });
            return tempateResult;
        }

        public EmployeeReviewModel CreateReviewMst(EmployeeReviewModel model, DBHelper dbHelper)
        {

            TryCatch.Run(() =>
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("EmpReviewId", model.EmpReviewId, DbType.Int32, ParameterDirection.InputOutput));
                paramCollection.Add(new DBParameter("EmpId", model.EmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserEmpId", model.AppraiserEmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("OutcomeValue", model.OutcomeValue, DbType.Int32));
                paramCollection.Add(new DBParameter("GradeId", model.GradeId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserStatus", model.AppraiserStatus, DbType.Int32));
                paramCollection.Add(new DBParameter("Comments", model.Comments, DbType.String));
                paramCollection.Add(new DBParameter("GradeRemark", model.GradeRemark, DbType.String));
                paramCollection.Add(new DBParameter("InsertedBy", model.InsertedBy, DbType.Int32));
                paramCollection.Add(new DBParameter("InsertedOn", model.InsertedOn, DbType.DateTime));
                paramCollection.Add(new DBParameter("InsertedIpAddress", model.InsertedIpAddress, DbType.String));
                paramCollection.Add(new DBParameter("InsertedMacName", model.InsertedMacName, DbType.String));
                paramCollection.Add(new DBParameter("InsertedMacId", model.InsertedMacId, DbType.String));
                paramCollection.Add(new DBParameter("GradeScore", model.GradeScore, DbType.Decimal));
                paramCollection.Add(new DBParameter("Year", model.Year, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserDesgId", model.AppraiserDesgId, DbType.Int32));
                paramCollection.Add(new DBParameter("QuestionnaireId", model.QuestionnaireId, DbType.Int32));
                var param = dbHelper.ExecuteNonQueryForOutParameter<int>(ReviewQueries.CreateReviewMst, paramCollection, CommandType.StoredProcedure, "EmpReviewId");
                model.EmpReviewId = param;
            }).IfNotNull(ex =>
            {
                _logger.LogError("error at CreateReviewMst" + ex.Message + Environment.NewLine + ex.StackTrace);
            });
            return model;
        }

        public void CreateReviewDtls(EmployeeReviewModel EmpReview, DBHelper dbHelper)
        {
            if (EmpReview.EmpReviewId > 0 && EmpReview.HeadList != null && EmpReview.HeadList.Count() > 0)
            {
                foreach (var detail in EmpReview.HeadList)
                {
                    CreateReviewDtls(EmpReview.EmpReviewId, detail, dbHelper);
                }
            }
            if (EmpReview.Recommendations != null)
            {
                CreateRecommnedations(EmpReview.EmpReviewId, EmpReview.Recommendations, dbHelper);
            }

            if (EmpReview.Components != null)
            {
                EmpReview.Components.EmpReviewId = EmpReview.EmpReviewId;
                EmpReview.Components.EmpId = EmpReview.EmpId;
                CreateFinalComponents(EmpReview.Components, dbHelper);
            }

        }

        public int UpdateReview(EmployeeReviewModel model, DBHelper dbHelper)
        {
            int tempateResult = 0;
            TryCatch.Run(() =>
            {
                var EmpReview = UpdateReviewMst(model, dbHelper);
                CreateReviewDtls(EmpReview, dbHelper);
            }).IfNotNull(ex =>
            {
                _logger.LogError("error at UpdateReview" + ex.Message + Environment.NewLine + ex.StackTrace);
            });
            return tempateResult;

        }

        public EmployeeReviewModel UpdateReviewMst(EmployeeReviewModel model, DBHelper dbHelper)
        {

            TryCatch.Run(() =>
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("EmpReviewId", model.EmpReviewId, DbType.Int32));
                paramCollection.Add(new DBParameter("EmpId", model.EmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserEmpId", model.AppraiserEmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("OutcomeValue", model.OutcomeValue, DbType.Int32));
                paramCollection.Add(new DBParameter("GradeId", model.GradeId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserStatus", model.AppraiserStatus, DbType.Int32));
                paramCollection.Add(new DBParameter("Comments", model.Comments, DbType.String));
                paramCollection.Add(new DBParameter("GradeRemark", model.GradeRemark, DbType.String));
                paramCollection.Add(new DBParameter("UpdatedBy", model.InsertedBy, DbType.Int32));
                paramCollection.Add(new DBParameter("UpdatedOn", model.InsertedOn, DbType.DateTime));
                paramCollection.Add(new DBParameter("UpdatedIPAddress", model.InsertedIpAddress, DbType.String));
                paramCollection.Add(new DBParameter("UpdatedMacName", model.InsertedMacName, DbType.String));
                paramCollection.Add(new DBParameter("UpdatedMacID", model.InsertedMacId, DbType.String));
                paramCollection.Add(new DBParameter("GradeScore", model.GradeScore, DbType.Decimal));
                paramCollection.Add(new DBParameter("AppraiserDesgId", model.AppraiserDesgId, DbType.Int32));
                paramCollection.Add(new DBParameter("QuestionnaireId", model.QuestionnaireId, DbType.Int32));
                var param = dbHelper.ExecuteNonQuery(ReviewQueries.UpdateReviewMst, paramCollection, CommandType.StoredProcedure);              
            }).IfNotNull(ex =>
            {
                _logger.LogError("error at UpdateReviewMst" + ex.Message + Environment.NewLine + ex.StackTrace);
            });
            return model;
        }

        public void CreateFinalComponents(EmpComponents Components, DBHelper dbHelper)
        {
            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("EmpReviewId", Components.EmpReviewId, DbType.Int32));
            paramCollection.Add(new DBParameter("EmpId", Components.EmpId, DbType.Int32));
            paramCollection.Add(new DBParameter("PreviousGross", Components.PreviousGross, DbType.Double));
            paramCollection.Add(new DBParameter("PreviousIncrement", Components.PreviousIncrement, DbType.Double));
            paramCollection.Add(new DBParameter("PreviousDate", Components.PreviousDate, DbType.DateTime));
            paramCollection.Add(new DBParameter("LastGross", Components.LastGross, DbType.Double));
            paramCollection.Add(new DBParameter("Increment", Components.Increment, DbType.Double));
            paramCollection.Add(new DBParameter("NewGross", Components.NewGross, DbType.Double));
            paramCollection.Add(new DBParameter("IncrementDate", Components.IncrementDate, DbType.DateTime));
            paramCollection.Add(new DBParameter("LastDesgId", Components.LastDesgId, DbType.Int32));
            paramCollection.Add(new DBParameter("NewDesgID", Components.NewDesgID, DbType.Int32));
            paramCollection.Add(new DBParameter("LastDeptId", Components.LastDeptId, DbType.Int32));
            dbHelper.ExecuteNonQuery(ReviewQueries.CreateFinalComponents, paramCollection, CommandType.StoredProcedure);
        }

        public void UpdateFinalComponents(EmpComponents Components, DBHelper dbHelper)
        {
            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("EmpReviewId", Components.EmpReviewId, DbType.Int32));
            paramCollection.Add(new DBParameter("EmpId", Components.EmpId, DbType.Int32));
            paramCollection.Add(new DBParameter("Increment", Components.Increment, DbType.Double));
            paramCollection.Add(new DBParameter("NewGross", Components.NewGross, DbType.Double));
            paramCollection.Add(new DBParameter("IncrementDate", Components.IncrementDate, DbType.DateTime));
            paramCollection.Add(new DBParameter("NewDesgID", Components.NewDesgID, DbType.Int32));
            dbHelper.ExecuteNonQuery(ReviewQueries.UpdateFinalComponents, paramCollection, CommandType.StoredProcedure);
        }


        public void CreateReviewDtls(int EmpReviewId, EmpReviewHeads Subhead, DBHelper dbHelper)
        {

            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("EmpReviewId", EmpReviewId, DbType.Int32));
            paramCollection.Add(new DBParameter("SubHeadId", Subhead.SubHeadId, DbType.Int32));
            paramCollection.Add(new DBParameter("PointGiven", Subhead.PointGiven, DbType.Double));
            paramCollection.Add(new DBParameter("HeadsRevView", Subhead.HeadsRevView, DbType.String));
            var parameterList = dbHelper.ExecuteNonQuery(ReviewQueries.CreateReviewDtls, paramCollection, CommandType.StoredProcedure);
        }

        public void CreateRecommnedations(int EmpReviewId, EmpRecommendations Recommendations, DBHelper dbHelper)
        {
            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("EmpReviewId", EmpReviewId, DbType.Int32));
            paramCollection.Add(new DBParameter("RecommendationValue", Recommendations.RecommendationValue, DbType.Int32));
            paramCollection.Add(new DBParameter("TrainingNeeds", Recommendations.TrainingNeeds, DbType.String));
            paramCollection.Add(new DBParameter("RecDesignationId", Recommendations.RecDesignationId, DbType.Int32));
            paramCollection.Add(new DBParameter("RecSalary", Recommendations.RecSalary, DbType.Double));
            paramCollection.Add(new DBParameter("RecIncrment", Recommendations.RecIncrment, DbType.Double));
            var parameterList = dbHelper.ExecuteNonQuery(ReviewQueries.CreateRecommnedations, paramCollection, CommandType.StoredProcedure);
        }

        

        public List<EmployeeReviewModel> GetRvwdEmployees(int UserId,int Year)
        {
            List<EmployeeReviewModel> EmpRvwd = null;
            
            TryCatch.Run(() =>
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32)); 
                paramCollection.Add(new DBParameter("Year", Year, DbType.Int32));
                using (DBHelper dbHelper = new DBHelper())
                {
                    DataTable dtRvwdEmployees = dbHelper.ExecuteDataTable(ReviewQueries.GetRvwdEmployees, paramCollection, CommandType.StoredProcedure);
                    EmpRvwd = dtRvwdEmployees.AsEnumerable()
                   .Select(row => new EmployeeReviewModel
                   {
                       EmpCode = row.Field<string>("EmpCode"),
                       EmpReviewId = row.Field<int>("EmpReviewId"),
                       EmpId = row.Field<int>("EmpId"),
                       EmpName = row.Field<string>("EmpName"),
                       StrJoiningDate = row.Field<string>("JoiningDate"),
                       StrConfirmationDate = row.Field<string>("ConfirmationDate"),
                       StrLastPromoDate = row.Field<string>("LastPromoDate"),
                       Salary = row.Field<double>("Salary"),
                       AppraiserEmpId = row.Field<int>("AppraiserEmpId"),
                       DesignationName = row.Field<string>("DesignationName"),
                       AppraiserDesigName = row.Field<string>("AppraiserDesigName"),
                       AppraiserName = row.Field<string>("AppraiserName"),
                       GradePoints = row.Field<int>("GradePoints"),
                       OutcomeValue = (OutcomeEnum)row.Field<int>("OutcomeValue"),
                       GradeName = row.Field<string>("GradeName"),
                       GradeRemark = row.Field<string>("GradeRemark"),
                       GradeScore = row.Field<decimal>("GradeScore"),
                       DesignationId = row.Field<int>("DesignationId"),
                       QuestionnaireId = row.Field<int>("QuestionnaireId"),
                       Recommendations  = new EmpRecommendations(
                           (RecommendationsEnum)row.Field<int>("RecommendationValue"),
                            row.Field<string>("TrainingNeeds"),
                            row.Field<int>("RecDesignationId"),
                            row.Field<double>("RecSalary"),
                            row.Field<double>("RecIncrment")),
                       Components = new EmpComponents(
                            row.Field<double>("PreviousGross"),
                            row.Field<double>("PreviousIncrement")),
                       DeptId = row.Field<int>("DeptId"),
                       Comments = row.Field<string>("Comments"),
                       ApprOneStatus = row.Field<string>("ApprOneStatus"),
                       ApprTwoStatus = row.Field<string>("ApprTwoStatus"),
                       IsShwBtn = row.Field<bool>("IsShwBtn"),
                       Year = row.Field<int>("Year"),
                       AppraiserDesgId = row.Field<int>("AppraiserDesgId")
                   }).ToList();
                }

            }).IfNotNull(ex =>
            {
                _logger.LogError("error at GetRvwdEmployees" + ex.Message + Environment.NewLine + ex.StackTrace);
            });

            return EmpRvwd;
        }

        public List<EmployeeReviewModel> GetRvwdEmps(int UserId,int Year)
        {
            List<EmployeeReviewModel> EmpRvwd = null;

            TryCatch.Run(() =>
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
                paramCollection.Add(new DBParameter("Year", Year, DbType.Int32));
                using (DBHelper dbHelper = new DBHelper())
                {
                    DataTable dtRvwdEmployees = dbHelper.ExecuteDataTable(ReviewQueries.GetRvwdEmps, paramCollection, CommandType.StoredProcedure);
                    EmpRvwd = dtRvwdEmployees.AsEnumerable()
                   .Select(row => new EmployeeReviewModel
                   {
                       EmpCode = row.Field<string>("EmpCode"),
                       EmpFinalReviewId = row.Field<int>("EmpFinalReviewId"), 
                       EmpReviewId = row.Field<int>("EmpReviewId"),
                       EmpId = row.Field<int>("EmpId"),
                       EmpName = row.Field<string>("EmpName"),
                       AppraiserTwoEmpId =  row.Field<int> ("AppraiserTwoEmpId"),
                       StrJoiningDate = row.Field<string>("JoiningDate"),
                       StrConfirmationDate = row.Field<string>("ConfirmationDate"),
                       StrLastPromoDate = row.Field<string>("LastPromoDate"),
                       Salary = row.Field<double>("Salary"),
                       AppraiserEmpId = row.Field<int>("AppraiserEmpId"),
                       DesignationName = row.Field<string>("DesignationName"),
                       AppraiserDesigName = row.Field<string>("AppraiserDesigName"),
                       AppraiserName = row.Field<string>("AppraiserName"),
                       GradePoints = row.Field<int>("GradePoints"),
                       OutcomeValue = (OutcomeEnum)row.Field<int>("OutcomeValue"),
                       GradeName = row.Field<string>("GradeName"),
                       Comments = row.Field<string>("Comments"),
                       OutComeManView = row.Field<string>("OutComeManView"),
                       AppraiserTwoComments = row.Field<string>("AppraiserTwoComments"),
                       GradeRemark = row.Field<string>("GradeRemark"),
                       GradeScore = row.Field<decimal>("GradeScore"),
                       DesignationId = row.Field<int>("DesignationId"),
                       QuestionnaireId = row.Field<int>("QuestionnaireId"),
                       Recommendations = new EmpRecommendations(
                           (RecommendationsEnum)row.Field<int>("RecommendationValue"),
                            row.Field<string>("TrainingNeeds"),
                            row.Field<int>("RecDesignationId"),
                            row.Field<double>("RecSalary"),
                            row.Field<double>("RecIncrment"),
                            row.Field<string>("RecManView")),                                                                                         
                       DeptId = row.Field<int>("DeptId"),
                       ApprTwoStatus = row.Field<string>("ApprTwoStatus"),
                       IsShwBtn = row.Field<bool>("IsShwBtn"),
                       AppraiserDesgId = row.Field<int>("AppraiserDesgId")
                   }).ToList();

                }

            }).IfNotNull(ex =>
            {
                _logger.LogError("error at GetRvwdEmps" + ex.Message + Environment.NewLine + ex.StackTrace);
            });

            return EmpRvwd;
        }



        public int CreateFinalReview(EmployeeReviewModel model, DBHelper dbHelper)
        {
            int tempateResult = 0;
            TryCatch.Run(() =>
            {
                var EmpReview = CreateFinalReviewMst(model, dbHelper);
                CreateFinalReviewDtls(EmpReview, dbHelper);
                tempateResult = 1;

            }).IfNotNull(ex =>
            {
                _logger.LogError("error at CreateFinalReview" + ex.Message + Environment.NewLine + ex.StackTrace);
            });
            return tempateResult;
        }

        public EmployeeReviewModel CreateFinalReviewMst(EmployeeReviewModel model, DBHelper dbHelper)
        {

            TryCatch.Run(() =>
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("EmpFinalReviewId", model.EmpFinalReviewId, DbType.Int32, ParameterDirection.InputOutput));
                paramCollection.Add(new DBParameter("EmpReviewId", model.EmpReviewId, DbType.Int32));
                paramCollection.Add(new DBParameter("EmpId", model.EmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserTwoEmpId", model.AppraiserTwoEmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("OutcomeValue", model.OutcomeValue, DbType.Int32));
                paramCollection.Add(new DBParameter("OutComeManView", model.OutComeManView, DbType.String));                
                paramCollection.Add(new DBParameter("GradeId", model.GradeId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserTwoStatus", model.AppraiserTwoStatus, DbType.Int32));
                paramCollection.Add(new DBParameter("Comments", model.AppraiserTwoComments, DbType.String));
                paramCollection.Add(new DBParameter("GradeRemark", model.GradeRemark, DbType.String));
                paramCollection.Add(new DBParameter("InsertedBy", model.InsertedBy, DbType.Int32));
                paramCollection.Add(new DBParameter("InsertedOn", model.InsertedOn, DbType.DateTime));
                paramCollection.Add(new DBParameter("InsertedIpAddress", model.InsertedIpAddress, DbType.String));
                paramCollection.Add(new DBParameter("InsertedMacName", model.InsertedMacName, DbType.String));
                paramCollection.Add(new DBParameter("InsertedMacId", model.InsertedMacId, DbType.String));
                paramCollection.Add(new DBParameter("RecommendationValue", model.Recommendations.RecommendationValue, DbType.Int32));
                paramCollection.Add(new DBParameter("GradeScore", model.GradeScore, DbType.Decimal));
                paramCollection.Add(new DBParameter("AppraiserDesgId", model.AppraiserDesgId, DbType.Int32));
                var param = dbHelper.ExecuteNonQueryForOutParameter<int>(ReviewQueries.CreateFinalReviewMst, paramCollection, CommandType.StoredProcedure, "EmpFinalReviewId");
                model.EmpFinalReviewId = param;
            }).IfNotNull(ex =>
            {
                _logger.LogError("error at CreateFinalReviewMst" + ex.Message + Environment.NewLine + ex.StackTrace);
            });
            return model;
        }

        public void CreateFinalReviewDtls(EmployeeReviewModel EmpReview, DBHelper dbHelper)
        {
            if (EmpReview.EmpFinalReviewId > 0 && EmpReview.HeadList != null && EmpReview.HeadList.Count() > 0)
            {
                //var list = EmpReview.HeadList.Where(x => x.HeadsManView != null).ToList();
                foreach (var detail in EmpReview.HeadList)
                {                   
                    UpdateFinalReviewDtl(EmpReview.EmpReviewId, detail, dbHelper);
                }
            }

            if (EmpReview.Recommendations != null)
            {
                CreateFinalRecommned(EmpReview.EmpFinalReviewId, EmpReview.Recommendations, dbHelper);
            }

            if (EmpReview.Components != null)
            {
                EmpReview.Components.EmpReviewId = EmpReview.EmpReviewId;
                EmpReview.Components.EmpId = EmpReview.EmpId;
                UpdateFinalComponents(EmpReview.Components, dbHelper);
            }


        }

        public void UpdateFinalReviewDtl(int EmpReviewId, EmpReviewHeads Subhead, DBHelper dbHelper)
        {

            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("EmpReviewId", EmpReviewId, DbType.Int32));
            paramCollection.Add(new DBParameter("SubHeadId", Subhead.SubHeadId, DbType.Int32));
            paramCollection.Add(new DBParameter("SecondPointGiven", Subhead.PointGiven, DbType.Double));
            paramCollection.Add(new DBParameter("HeadsManView", Subhead.HeadsManView, DbType.String));
            var parameterList = dbHelper.ExecuteNonQuery(ReviewQueries.UpdateFinalReviewDtl, paramCollection, CommandType.StoredProcedure);
        }

        public void CreateFinalRecommned(int EmpFinalReviewId, EmpRecommendations Recommendations, DBHelper dbHelper)
        {
            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("EmpFinalReviewId", EmpFinalReviewId, DbType.Int32));
            paramCollection.Add(new DBParameter("RecommendationValue", Recommendations.RecommendationValue, DbType.Int32));
            paramCollection.Add(new DBParameter("TrainingNeeds", Recommendations.TrainingNeeds, DbType.String));
            paramCollection.Add(new DBParameter("RecDesignationId", Recommendations.RecDesignationId, DbType.Int32));
            paramCollection.Add(new DBParameter("RecSalary", Recommendations.RecSalary, DbType.Double));
            paramCollection.Add(new DBParameter("RecIncrment", Recommendations.RecIncrment, DbType.Double));
            paramCollection.Add(new DBParameter("RecManView", Recommendations.RecManView, DbType.String));
            var parameterList = dbHelper.ExecuteNonQuery(ReviewQueries.CreateFinalRecommned, paramCollection, CommandType.StoredProcedure);
        }

        public int UpdateFinalReview(EmployeeReviewModel model, DBHelper dbHelper)
        {
            int tempateResult = 0;
            TryCatch.Run(() =>
            {
                var EmpReview = UpdateFinalReviewMst(model, dbHelper);
                CreateFinalReviewDtls(EmpReview, dbHelper);
            }).IfNotNull(ex =>
            {
                _logger.LogError("error at UpdateFinalReview" + ex.Message + Environment.NewLine + ex.StackTrace);
            });
            return tempateResult;

        }

        public EmployeeReviewModel UpdateFinalReviewMst(EmployeeReviewModel model, DBHelper dbHelper)
        {

            TryCatch.Run(() =>
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("EmpFinalReviewId", model.EmpFinalReviewId, DbType.Int32));
                paramCollection.Add(new DBParameter("EmpReviewId", model.EmpReviewId, DbType.Int32));
                paramCollection.Add(new DBParameter("EmpId", model.EmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserTwoEmpId", model.AppraiserTwoEmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("OutcomeValue", model.OutcomeValue, DbType.Int32));
                paramCollection.Add(new DBParameter("OutComeManView", model.OutComeManView, DbType.String));
                paramCollection.Add(new DBParameter("GradeId", model.GradeId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserTwoStatus", model.AppraiserTwoStatus, DbType.Int32));
                paramCollection.Add(new DBParameter("Comments", model.AppraiserTwoComments, DbType.String));
                paramCollection.Add(new DBParameter("GradeRemark", model.GradeRemark, DbType.String));
                paramCollection.Add(new DBParameter("UpdatedBy", model.InsertedBy, DbType.Int32));
                paramCollection.Add(new DBParameter("UpdatedOn", model.InsertedOn, DbType.DateTime));
                paramCollection.Add(new DBParameter("UpdatedIPAddress", model.InsertedIpAddress, DbType.String));
                paramCollection.Add(new DBParameter("UpdatedMacName", model.InsertedMacName, DbType.String));
                paramCollection.Add(new DBParameter("UpdatedMacID", model.InsertedMacId, DbType.String));
                paramCollection.Add(new DBParameter("RecommendationValue", model.Recommendations.RecommendationValue, DbType.Int32));
                paramCollection.Add(new DBParameter("GradeScore", model.GradeScore, DbType.Decimal));
                paramCollection.Add(new DBParameter("AppraiserDesgId", model.AppraiserDesgId, DbType.Int32));
                var param = dbHelper.ExecuteNonQuery(ReviewQueries.UpdateFinalReviewMst, paramCollection, CommandType.StoredProcedure);
            }).IfNotNull(ex =>
            {
                _logger.LogError("error at UpdateFinalReviewMst" + ex.Message + Environment.NewLine + ex.StackTrace);
            });
            return model;
        }


       







    }


}