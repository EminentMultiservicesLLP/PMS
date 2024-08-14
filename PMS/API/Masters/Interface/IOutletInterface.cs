using PMS.Areas.Masters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.API.Masters.Interface
{
    public interface IOutletInterface
    {
        int CreateNewOutletMasters(OutletModel model);
        IEnumerable<OutletModel> GetAllOutlet(int UserId);
        IEnumerable<OutletModel> GetAllActiveOutlet(int UserId);
        bool CheckDuplicateItem(string OutletName, string type);
        bool CheckDuplicateUpdate(OutletModel model, string type);
        bool UpdateOutletMasters(OutletModel model);
    }
}