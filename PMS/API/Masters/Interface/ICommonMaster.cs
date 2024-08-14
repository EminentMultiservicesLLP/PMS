using PMS.Areas.Masters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.API.Masters.Interface
{
    public interface ICommonMaster
    {
        List<CommonMasterModel> GetAllDesignation();
        List<CommonMasterModel> GetAllGrade();
        List<CommonMasterModel> GetAllOutlet();
    }
}
