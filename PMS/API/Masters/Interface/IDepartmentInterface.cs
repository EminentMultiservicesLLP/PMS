using PMS.Areas.Masters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.API.Masters.Interface
{
    
        public interface IDepartmentInterface
        {
            int CreateNewDepartmentMasters(DepartmentModel model);
            IEnumerable<DepartmentModel> GetAllDepartment(int UserId);
            IEnumerable<DepartmentModel> GetAllActiveDepartment(int UserId);
            bool CheckDuplicateItem(string DepartmentName, string type);
            //bool CheckDuplicateUpdate(string typecode, string type);
            bool CheckDuplicateUpdate(DepartmentModel model, string type);

            bool UpdateDepartmentMasters(DepartmentModel model);

        }
    
}
