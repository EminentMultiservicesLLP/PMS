using PMS.Areas.Masters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.API.Masters.Interface
{
    public interface IEmployeeMstInterface
    {
        IEnumerable<EmployeeModel> GetAllEmployee(int UserId);
        IEnumerable<EmployeeModel> GetAllActiveEmployee(int UserId);
        IEnumerable<EmployeeModel> GetAllRole();
        int CreateNewEmployeeMasters(EmployeeModel model);
        int CreateNewUserMaster(EmployeeModel model,int EmpId, bool IsExist);
        bool UpdateEmployeeMasters(EmployeeModel model);
        bool CheckDuplicateItem(string callfrom,int EmpId,string EmpCode, string type);
        bool CheckDuplicateUser(int EmpId);
        void DeactivateUser(int EmpId, int RoleId);
    }
}