using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Models;


namespace PMS.API.LoginPanel.Interface
{
    public interface ILoginPageRepository
    {
        int SaveLoginPage(LoginPageModel model);
      
    }
}
