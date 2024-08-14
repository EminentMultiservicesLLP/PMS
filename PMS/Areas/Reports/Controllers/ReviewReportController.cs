using CommonLayer;
using PMS.API.Reports.Interface;
using PMS.Areas.Reports.Models;
using PMS.API.Reports.Repository;
using PMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.Areas.Masters.Models;

namespace PMS.Areas.Reports.Controllers
{
    [CheckSession]
    public class ReviewReportController : Controller
    {

        IReviewReports _reviewReport;
        private static readonly ILogger _logger = Logger.Register(typeof(ReviewReportController));

        public ReviewReportController(IReviewReports reviewReport)
        {
            _reviewReport = reviewReport;
        }

        public ReviewReportController()
        {
            _reviewReport = new ReviewReportsRepository();
        }


        public ReviewReportsModel GetReviewReport(int rvwtype, int outletId, int gradeId, int desgId, int empId)
        {
            try
            {
                int Year = 2019;
                ReviewReportsModel ResultData = new ReviewReportsModel();
                ResultData.EmpRvwData = _reviewReport.GetReviewReport(rvwtype, outletId, gradeId, desgId, empId, Year);
                return ResultData;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ReviewReportsModel GetReviewReport :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }


        public List<ReviewReportHeads> GetReportHeads( int EmpReviewId,int RvwType)
        {
            try
            {
                List<ReviewReportHeads> list = new List<ReviewReportHeads>();
                list = _reviewReport.GetReportHeads(EmpReviewId, RvwType);
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ReviewReportsModel GetReportHeads :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }

        public ActionResult GetAllEmployee()
        {
        
            JsonResult jsonResult = null;
            try
            {
                string RoleId = Convert.ToString(Session["RoleId"]); 
                List<EmployeeModel> list = new List<EmployeeModel>();
                list = _reviewReport.GetAllEmployee();
                jsonResult = Json(new { data = list, Role = RoleId }, JsonRequestBehavior.AllowGet);            
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ReviewReportsModel GetAllEmployee :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            return jsonResult;
        }

        public FinalReviewedReportModel GetFinalRvwdRpt(int OutletId,int GradeId,int DesgId,int  EmpId)
        {
            try
            {
                int Year = 2019;
                FinalReviewedReportModel ResultData = new FinalReviewedReportModel();
                ResultData.FinalRvwdData = _reviewReport.GetFinalRvwdRpt(OutletId, GradeId, DesgId, EmpId, Year);
                return ResultData;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ReviewReportsModel GetFinalRvwdRpt :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }


        public StatusReportModel GetStatusRpt(int OutletId, int GradeId, int DesgId)
        {
            try
            {
                int Year = 2019;
                StatusReportModel ResultData = new StatusReportModel();
                ResultData.EmpRvwStatus = _reviewReport.GetStatusRpt(OutletId, GradeId, DesgId,  Year);
                return ResultData;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ReviewReportsModel GetFinalRvwdRpt :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        





    }
}
