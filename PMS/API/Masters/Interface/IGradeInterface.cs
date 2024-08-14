using PMS.Areas.Masters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.API.Masters.Interface
{
    public interface IGradeInterface
    {
        int CreateNewGradeMasters(GradeModel model);
        IEnumerable<GradeModel> GetAllGrade(int UserId);
        IEnumerable<GradeModel> GetAllActiveGrade(int UserId);
        bool CheckDuplicateItem(string DesignationName, string type);
        bool CheckDuplicateUpdate(GradeModel model, string type);
        bool UpdateGradeMasters(GradeModel model);
    }
}