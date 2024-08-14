using CommonDataLayer.DataAccess;
using CommonLayer;
using CommonLayer.Extensions;
using PMS.API.Review.Interface;
using PMS.API.Review.Repository;
using PMS.Areas.Review.Models;
using PMS.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Areas.Review.Controllers
{
    [CheckSession]
    [ValidateHeaderAntiForgeryTokenAttribute]
    public class SecondReviewController : Controller
    {

        IEmployeeReview _EmpReview;
        private static readonly ILogger Loggger = Logger.Register(typeof(SecondReviewController));
        //
        // GET: /Review/SecondReview/


        public SecondReviewController()
        {          
            _EmpReview = new EmployeeReviewRepository();
        }
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetRvwdEmps()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {
                int UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                int Year = 2019; 
                var list = _EmpReview.GetRvwdEmps(UserId, Year);
                jResult = Json(new { rvdEmployees = list }, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>
            {
                Loggger.LogError("Error in class:SecondReviewController method :GetRvwdEmployee :" + ex);
            });
            return jResult;
        }


        [HttpPost]
        public ActionResult CreateFinalReview(EmployeeReviewModel jsonData)
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
                    jsonData.AppraiserTwoStatus = jsonData.IsSubmit ? (int)Status.Submit : (int)Status.Save;
                    jsonData = CalculateSalaries(jsonData);
                    if (jsonData.EmpFinalReviewId == 0)
                    {                         
                         _EmpReview.CreateFinalReview(jsonData, dbHelper);
                    }
                    else
                    {                        
                         _EmpReview.UpdateFinalReview(jsonData, dbHelper);
                    }

                    dbHelper.CommitTransaction(transaction);
                    Success = true;

                }).IfNotNull(ex =>
                {
                    dbHelper.RollbackTransaction(transaction);
                    Loggger.LogError("Error in class:SecondReviewController method : CreateSecReview" + ex.Message + Environment.NewLine + ex.StackTrace);
                });
                return Json(new { success = Success, message = "Employee Review Completed" }, JsonRequestBehavior.AllowGet);
            }
        }


        public EmployeeReviewModel CalculateSalaries(EmployeeReviewModel jsonData)
        {
            TryCatch.Run(() =>
            {
                if ((int)jsonData.Recommendations.RecommendationValue == 2)
                {
                    jsonData.Components.Increment = jsonData.Recommendations.RecSalary - jsonData.Components.LastGross;
                    
                }
                else if ((int)jsonData.Recommendations.RecommendationValue == 3)
                {
                    jsonData.Components.Increment = jsonData.Recommendations.RecIncrment;                   
                }
                else
                {
                    jsonData.Components.Increment = 0;                    
                }
                jsonData.Components.IncrementDate = DateTime.Now;
                jsonData.Components.NewGross = jsonData.Components.Increment + jsonData.Components.LastGross;              

            }).IfNotNull(ex =>
            {
                Loggger.LogError("Error in class:FirstReviewController method : CalculateSalaries" + ex.Message + Environment.NewLine + ex.StackTrace);
            });
            return jsonData;
        }



    }
}
