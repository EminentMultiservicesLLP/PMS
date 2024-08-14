using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Areas.Masters.Models;

namespace PMS.API.Masters.Interface
{
    public interface IEmployee
    {
        List<EmployeeModel> GetEmpForReview( int UserId);
    }
}
