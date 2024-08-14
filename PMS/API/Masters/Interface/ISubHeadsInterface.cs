using PMS.Areas.Masters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.API.Masters.Interface
{
    public interface ISubHeadsInterface
    {
        IEnumerable<SubheadsModel> GetAllSubHeads(int UserId);
        IEnumerable<SubheadsModel> GetAllHeadName(int Deptid);

        IEnumerable<SubheadsModel> GetAllActiveSubHeads(int UserId);

        int CreateNewSubHeadsMasters(SubheadsModel model);
        bool CheckDuplicateItem(string SubHeadName, int HeadId, string type);
        bool CheckDuplicateUpdate(string SubHeadName, int HeadId, string type);
        bool UpdateSubHeadsMasters(SubheadsModel model);
    }
}