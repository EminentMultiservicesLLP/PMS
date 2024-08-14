using CommonDataLayer.DataAccess;
using PMS.Areas.Masters.Models;
using PMS.Areas.Review.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.API.Review.Interface
{
    public interface IEmployeeReview
    {
        List<SubheadsModel> GetHeads(int QuestionnaireId, int EmpReviewId);   
        int CreateReview(EmployeeReviewModel model, DBHelper dbHelper);
        List<EmployeeReviewModel> GetRvwdEmployees(int UserId,int Year);
        List<EmployeeReviewModel> GetRvwdEmps(int UserId,int Year);
        int UpdateReview(EmployeeReviewModel model, DBHelper dbHelper);
        int CreateFinalReview(EmployeeReviewModel model, DBHelper dbHelper);
        int UpdateFinalReview(EmployeeReviewModel model, DBHelper dbHelper);        

    }
}
