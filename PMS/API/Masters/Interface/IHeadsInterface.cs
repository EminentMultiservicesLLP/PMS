using PMS.Areas.Masters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.API.Masters.Interface
{
    public interface IHeadsInterface
    {
        int CreateNewHeadsMasters(HeadsModel model);
        bool CheckDuplicateItem(string HeadName, int DeptId, string type);
        bool CheckDuplicateUpdate(string typecode,int DeptId, string type);
        bool UpdateHeadsMasters(HeadsModel model);

        IEnumerable<HeadsModel> GetAllHeads();
        IEnumerable<HeadsModel> GetAllActiveHeads();
    }
}