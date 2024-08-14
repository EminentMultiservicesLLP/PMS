using PMS.Areas.Masters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.API.Masters.Interface
{
    public interface IUserInterface
    {
        IEnumerable<UserModel> GetAllUser();
        bool CheckDuplicateUpdate(int UserId,int EmpId, string type);
        bool UpdateUserMasters(UserModel model);
    }
}