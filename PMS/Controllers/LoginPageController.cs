using PMS.API.LoginPanel.Interface;
using PMS.API.LoginPanel.Repository;
using PMS.Caching;
using PMS.Models;
using CommonLayer;
using CommonLayer.EncryptDecrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PMS.Controllers
{
    public class LoginPageController : Controller
    {
        ILoginPageRepository _action;

        private static readonly ILogger _loggger = Logger.Register(typeof(LoginPageController));
     
        // GET: /LoginPanel/


        public LoginPageController(ILoginPageRepository action)
        {
            _action = action;
        }
        public LoginPageController()
        {
            _action = new LoginPageRepository();
        }
        
        [HttpPost]
        public ActionResult SaveDetails(LoginPageModel model)
        {
            JsonResult jlResult; bool success = false;
            try
            {
               
                var saveuser = _action.SaveLoginPage(model);
                if (saveuser !=0)
                {
                    success = true;
                    jlResult = Json(new { message = "Record Saved Successfully!", success = true }, JsonRequestBehavior.AllowGet);
                    //_loggger.LogInfo("Loggin off to User:" + Session["AppUserId"].ToString());
                   // LoginModel Model = new LoginModel();
                   // var SessionId = System.Web.HttpContext.Current.Session.SessionID;
                   // Model.SetUserIsLogin(Convert.ToInt32(Session["AppUserId"].ToString()), false, SessionId);
                    FormsAuthentication.SignOut();                    
                    Session["AppUserId"] = "";
                    MemoryCaching.ClearAllCache();
                }
                else
                {
                    success = false;
                    jlResult= Json(new { success = false, message = "Record Save Failed" });
                }
            }
            catch (Exception ex)
            {
                _loggger.LogError("Error in LoginPageController SaveDetails" + ex.Message + Environment.NewLine + ex.StackTrace);
                jlResult = Json(new { message = "Something Went Wrong", success = false }, JsonRequestBehavior.AllowGet);
            }
            return jlResult;
        }
        [AllowAnonymous]
        public ActionResult FirstLogin(string username, int UserID)
        {
            return View();
        }

    }
}
