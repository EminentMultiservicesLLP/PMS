using PMS.Areas.Masters.Models;
using PMS.Areas.Reports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.API.Reports.Interface
{
    public  interface IReviewReports
    {
        List<ReviewReportsModel> GetReviewReport(int rvwtype, int outletId, int gradeId, int desgId, int empId,int Year);
        List<ReviewReportHeads> GetReportHeads( int EmpReviewId,int RvwType);
        List<EmployeeModel> GetAllEmployee();
        List<FinalReviewedReportModel> GetFinalRvwdRpt (int outletId, int gradeId, int desgId, int empId, int Year);
        List<StatusReportModel> GetStatusRpt(int outletId, int gradeId, int desgId, int Year);


    }
}
