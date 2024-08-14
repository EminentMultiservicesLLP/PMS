using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;

using System.Web.Security;
using PMS.Models;
using PMS.Filters;
using CommonLayer;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using CommonLayer.EncryptDecrypt;

namespace PMS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILogger _logger = Logger.Register(typeof(MvcApplication));
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_Error(object sender, EventArgs e)
        {

            var ex = Server.GetLastError();

             _logger.LogInfo("Application_Error Unhandled exception " + Convert.ToString(HttpContext.Current.Session["AppUserId"]) + " at :" + DateTime.Now.ToLongTimeString()+Environment.NewLine + ex.Message + Environment.NewLine +ex.StackTrace);
            string fromAddress = ConfigurationManager.AppSettings["mailUserID"];
            string fromPassword = EncryptDecryptDES.DecryptString(ConfigurationManager.AppSettings["mailPassword"]);
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = ConfigurationManager.AppSettings["smtpHost"];
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPortNo"]);
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["sslSecurityStatus"]);
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                smtp.Timeout = 20000;
            }

            MailAddress from = new MailAddress(fromAddress);      
            string[] MultipleAddress = { "Diwakar.singh@gmail.com", "info@eminentMultiservices.com", "ankitmane@live.in" };
            MailMessage message = new MailMessage();
            foreach (var address in MultipleAddress)
            {
                message.To.Add(address);
            }
            message.From = from;
            message.Body = ex.StackTrace;
            message.Subject = " **** Unhanhandled Exception occured in QAseries 2";
            //smtp.Send(message);

        }




        }
}