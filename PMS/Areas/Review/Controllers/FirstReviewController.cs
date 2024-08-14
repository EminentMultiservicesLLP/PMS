using CommonDataLayer.DataAccess;
using CommonLayer;
using CommonLayer.Extensions;
using PMS.API.Masters.Interface;
using PMS.API.Masters.Repository;
using PMS.API.Review.Interface;
using PMS.API.Review.Repository;
using PMS.Areas.Review.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using PMS.Filters;
using PMS.Reports;

namespace PMS.Areas.Review.Controllers
{
    [CheckSession]
    [ValidateHeaderAntiForgeryTokenAttribute]
    public class FirstReviewController : Controller
    {
        //
        // GET: /Review/FirstReview/
        IEmployee _employee;
        IEmployeeReview _EmpReview;
        private static readonly ILogger Loggger = Logger.Register(typeof(FirstReviewController));

        public FirstReviewController()
        {
            _employee = new EmployeeRepository();
            _EmpReview = new EmployeeReviewRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetEmpForReview()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {
                int UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                var list = _employee.GetEmpForReview(UserId);
                Loggger.LogInfo("GetEmpForReview Request Completed at :" + DateTime.Now.ToLongTimeString());
                jResult = Json(new { Employees = list }, JsonRequestBehavior.AllowGet);
            }).IfNotNull(ex =>
            {
                Loggger.LogError("Error in class:FirstReviewController method :GetEmpForReview :" + ex);
            });

            return jResult;

        }

        [HttpGet]
        public ActionResult GetHeads(int QuestionnaireId, int EmpReviewId)
        {
            JsonResult jResult = null; double SubheadCount = 0;
            TryCatch.Run(() =>
            {
                var list = _EmpReview.GetHeads(QuestionnaireId, EmpReviewId);
                if (list.Count > 0)
                {
                    SubheadCount = Convert.ToDouble(list.Where(x => x.SubHeadId != 0).Count());
                }

                jResult = Json(new { SubHeads = list, subheadCount = SubheadCount }, JsonRequestBehavior.AllowGet);
            }).IfNotNull(ex =>
            {

                Loggger.LogError("Error in class:FirstReviewController method :GetHeadsByDeptId :" + ex);
            });
            return jResult;
        }

        

        [HttpGet]
        public ActionResult GetRvwdEmployees()
        {
            JsonResult jResult = null; 
            TryCatch.Run(() =>
            {
                int UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                int Year = 2019;
                var list = _EmpReview.GetRvwdEmployees(UserId, Year);
                jResult = Json(new { rvdEmployees = list }, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>
            {
                Loggger.LogError("Error in class:FirstReviewController method :GetRvwdEmployee :" + ex);
            });
            return jResult;
        }


        [HttpPost]
        public ActionResult CreateReview(EmployeeReviewModel jsonData)
        {
            bool Success = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                IDbTransaction transaction = dbHelper.BeginTransaction();
                TryCatch.Run(() =>
                {
                    jsonData.InsertedBy = Convert.ToInt32(Session["AppUserId"].ToString());
                    jsonData.InsertedOn = DateTime.Now;
                    jsonData.InsertedIpAddress = Common.Constants.IpAddress;
                    jsonData.InsertedMacId = Common.Constants.MacId;
                    jsonData.InsertedMacName = Common.Constants.MacName;
                    jsonData.AppraiserStatus = jsonData.IsSubmit ? (int)Status.Submit : (int)Status.Save;
                    jsonData.Year = 2019;
                    jsonData = CalculateSalaries(jsonData);
                    if (jsonData.EmpReviewId == 0)
                    {

                        _EmpReview.CreateReview(jsonData, dbHelper);
                    }
                    else
                    {
                        _EmpReview.UpdateReview(jsonData, dbHelper);
                    }


                    dbHelper.CommitTransaction(transaction);
                    Success = true;

                }).IfNotNull(ex =>
                {
                    dbHelper.RollbackTransaction(transaction);
                    Loggger.LogError("Error in class:FirstReviewController method : CreateReview" + ex.Message + Environment.NewLine + ex.StackTrace);
                });
            }

            return Json(new { success = Success, message = "Employee Review Completed" }, JsonRequestBehavior.AllowGet);
        }


        public EmployeeReviewModel CalculateSalaries(EmployeeReviewModel jsonData)
        {
            TryCatch.Run(() =>
            {
                if ((int)jsonData.Recommendations.RecommendationValue == 2)
                {
                    jsonData.Components.Increment = jsonData.Recommendations.RecSalary - jsonData.Components.LastGross;
                    jsonData.Components.IncrementDate = DateTime.Now;
                }
                else if ((int)jsonData.Recommendations.RecommendationValue == 3)
                {
                    jsonData.Components.Increment = jsonData.Recommendations.RecIncrment;
                    jsonData.Components.IncrementDate = DateTime.Now;
                }
                else
                {
                    jsonData.Components.Increment = 0;
                    jsonData.Components.IncrementDate = Convert.ToDateTime(jsonData.Components.StrPreviousDate);
                }

                jsonData.Components.NewGross = jsonData.Components.Increment + jsonData.Components.LastGross;
                jsonData.Components.PreviousDate = Convert.ToDateTime(jsonData.Components.StrPreviousDate);

            }).IfNotNull(ex =>
            {
                Loggger.LogError("Error in class:FirstReviewController method : CalculateSalaries" + ex.Message + Environment.NewLine + ex.StackTrace);
            });
            return jsonData;
        }


        [HttpGet]
        public ActionResult PrintRvwData( int Empid)
        {
            bool Success = true;
            ReportViewer rpt = new ReportViewer();
            rpt.PrintFromForm(Empid);
            return Json(new { success = Success, message = "Print Data " }, JsonRequestBehavior.AllowGet);
        }
        



    }
}
