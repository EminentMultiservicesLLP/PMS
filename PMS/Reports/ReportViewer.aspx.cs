using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Printing;

using CommonLayer;
using Microsoft.Reporting.WebForms;
using ReportDataSource = Microsoft.Reporting.WebForms.ReportDataSource;
using ReportParameter = Microsoft.Reporting.WebForms.ReportParameter;
using PMS.Models;
using PMS.Controllers;
using System.Linq;
using PMS.Areas.Reports.Controllers;
using PMS.Areas.Reports.Models;

namespace PMS.Reports
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        ReportParameter[] _rparams;
        private ReportDataSource _rds;

        static CommonLayer.ILogger _logger = Logger.Register(typeof(ReportViewer));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int ReportID = 0;

                if (Request.QueryString["reportid"] != null)
                {
                    ReportID = Convert.ToInt32(Request.QueryString["reportid"]);


                }

                if (ReportID == 1 || ReportID ==2 || ReportID == 3)
                {
                    string OtherParam = Request.QueryString["otherParam"];
                    var OtherValues = OtherParam.Split(',');
                    int rvwtype = Convert.ToInt32(OtherValues[0]);
                    int outletId = Convert.ToInt32(OtherValues[1]);
                    int gradeId = Convert.ToInt32(OtherValues[2]);
                    int desgId = Convert.ToInt32(OtherValues[3]);
                    int empId = Convert.ToInt32(OtherValues[4]);
                    if (ReportID == 1) GetReviewReport(rvwtype, outletId, gradeId, desgId, empId);
                    if (ReportID == 2) GetFinalRvwdRpt(outletId, gradeId, desgId, empId);
                    if (ReportID == 3) GetStatusRpt(outletId, gradeId, desgId);


                }                 

               


            }
        }

        private void GetReviewReport(int rvwtype, int outletId, int gradeId, int desgId, int empId)
        {
            ReviewReportController ReviewReport = new ReviewReportController();
            ReviewReportsModel Resultdata = ReviewReport.GetReviewReport(rvwtype, outletId, gradeId, desgId, empId);
            ReportViewer1.LocalReport.DataSources.Clear();
            string reportPath = Server.MapPath("../Reports/ReviewReports/AppraiserTwoReport.rdlc");
            ReportViewer1.LocalReport.ReportPath = reportPath;
            _rds = new ReportDataSource();
            _rds.Name = "DsReviewMst";
            _rds.Value = Resultdata.EmpRvwData;
            ReportViewer1.LocalReport.DataSources.Add(_rds);
            ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ReviewHeads);
            ReportViewer1.LocalReport.Refresh();

        }


        void ReviewHeads(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                ReviewReportController ReviewReport = new ReviewReportController();
                int  EmpReviewId = Convert.ToInt32(e.Parameters[0].Values[0]);
                //int DeptId = Convert.ToInt32(e.Parameters[1].Values[0]);
                int RvwType = Convert.ToInt32(e.Parameters[1].Values[0]);
                List<ReviewReportHeads> listHead = ReviewReport.GetReportHeads(EmpReviewId, RvwType);
                e.DataSources.Add(new ReportDataSource("DsReviewHeads", listHead));
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Report ReviewHeads failed:" + ex.Message + Environment.NewLine + ex.StackTrace);
            }

        }


        private void GetFinalRvwdRpt(int OutletId, int GradeId, int DesgId, int EmpId)
        {
            ReviewReportController ReviewReport = new ReviewReportController();
            FinalReviewedReportModel Resultdata = ReviewReport.GetFinalRvwdRpt(OutletId,  GradeId,  DesgId, EmpId);
            ReportViewer1.LocalReport.DataSources.Clear();
            string reportPath = Server.MapPath("../Reports/ReviewReports/ReviewFinal.rdlc");
            ReportViewer1.LocalReport.ReportPath = reportPath;
            _rds = new ReportDataSource();
            _rds.Name = "DsFinalReview";
            _rds.Value = Resultdata.FinalRvwdData;
            ReportViewer1.LocalReport.DataSources.Add(_rds);
            ReportViewer1.LocalReport.Refresh();


        }

        private void GetStatusRpt(int OutletId, int GradeId, int DesgId)
        {
            ReviewReportController ReviewReport = new ReviewReportController();
            StatusReportModel Resultdata = ReviewReport.GetStatusRpt(OutletId, GradeId, DesgId);
            ReportViewer1.LocalReport.DataSources.Clear();
            string reportPath = Server.MapPath("../Reports/ReviewReports/StatusReport.rdlc");
            ReportViewer1.LocalReport.ReportPath = reportPath;
            _rds = new ReportDataSource();
            _rds.Name = "DsStatus";
            _rds.Value = Resultdata.EmpRvwStatus;
            ReportViewer1.LocalReport.DataSources.Add(_rds);
            ReportViewer1.LocalReport.Refresh();


        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            ReportPrintDocument rp = new ReportPrintDocument(ReportViewer1.LocalReport);
            rp.Print();
        }


        public void PrintFromForm (int Empid)
        {
            GetFinalRvwdRpt(0, 0, 0, Empid);
            ReportPrintDocument rp = new ReportPrintDocument(ReportViewer1.LocalReport);
            rp.Print();

        }

    }



}