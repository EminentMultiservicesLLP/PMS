using PMS.Areas.Masters.Controllers;
using PMS.Areas.Masters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.API.Masters.Interface
{

    public interface IDesignationInterface
    {
        int CreateNewDesignationMasters(DesignationModel model);
        IEnumerable<DesignationModel> GetAllDesignation(int UserId);
        IEnumerable<DesignationModel> GetAllActiveDesignation(int UserId);
        bool CheckDuplicateUpdate(DesignationModel model, string type);
        bool CheckDuplicateItem(string DesignationName,string type);
        //bool CheckDuplicateUpdate(string typecode,string type);
        bool UpdateDesignationMasters(DesignationModel model);





    }
}
