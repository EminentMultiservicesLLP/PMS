using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using PMS.Models;
using System.Web;
using System.Web.Security;


namespace PMS.Filters
{




    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["AppUserId"] == null)
            {                             
                   filterContext = UserSignOut(filterContext);                                                                        
            }
            
            base.OnActionExecuting(filterContext);
        }

        public ActionExecutingContext UserSignOut (ActionExecutingContext filterContext)
        {
            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.Session.Abandon();
            filterContext.Result = new RedirectResult("~/Account/ReturnToLogin");            
            return filterContext;

        }


    }



}
