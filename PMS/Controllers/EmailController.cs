using PMS.Models;
using CommonLayer;
using CommonLayer.EncryptDecrypt;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PMS.Controllers
{
    public class EmailController : Controller
    {
        private static readonly ILogger _logger = Logger.Register(typeof(EmailController));
        public void SendInquiryEmail(EmailModel mails)
        {
            try
            {
                string toAddress = ConfigurationManager.AppSettings["mailToAddress"];
                string fromAddress = ConfigurationManager.AppSettings["mailUserID"];
                string fromPassword = EncryptDecryptDES.DecryptString(ConfigurationManager.AppSettings["mailPassword"]);
                // any address where the email will be sending
                //Password of your gmail address
                //const string toPassword = "mechcon@admin";
                // Passing the values and make a email formate to display
                string subject = mails.EmailSubject.ToString();
                string body = "From: " + mails.Name + "\n";
                body += "CompanyName: " + mails.CompanyName + "\n";
                body += "Email: " + mails.EmailFrom + "\n";
                body += "Subject: " + mails.EmailSubject + "\n";
                // body += "Message: \n" + mails.EmailSubject + "\n";
                // smtp settings
                //var smtp = new System.Net.Mail.SmtpClient();
                //{
                //    smtp.Host = "smtp.gmail.com";
                //    smtp.Port = 587;
                //    smtp.EnableSsl = true;
                //    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                //    smtp.Timeout = 20000;
                //}
                //// Passing values to smtp object
                ////smtp.Send(toAddress, toAddress, subject, body);
                //MailAddress from = new MailAddress(fromAddress);
                //MailAddress to = new MailAddress(toAddress);
                //MailMessage message = new MailMessage(from, to);
                //message.Body = body;
                //message.Subject = subject;
                //smtp.Send(message);

                var smtpdetails = SmtpSettings(toAddress, body, subject);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error In SendInquiryEmail: " + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public void SendFeedbackEmail(EmailModel mails)
        {
            try
            {
                string toAddress = ConfigurationManager.AppSettings["mailToAddress"];
                string fromAddress = ConfigurationManager.AppSettings["mailUserID"];
                string fromPassword = EncryptDecryptDES.DecryptString(ConfigurationManager.AppSettings["mailPassword"]);
                // var toAddress = "mechconmails@gmail.com";
                //Password of your gmail address
                //const string toPassword = "mechcon@admin";
                // Passing the values and make a email formate to display
                string subject = mails.EmailSubject.ToString();
                string body = "From: " + mails.Name + "\n";
                body += "Email: " + mails.EmailFrom + "\n";
                body += "Message: \n" + mails.EmailBody + "\n";
                // smtp settings
                //var smtp = new System.Net.Mail.SmtpClient();
                //{
                //    smtp.Host = "smtp.gmail.com";
                //    smtp.Port = 587;
                //    smtp.EnableSsl = true;
                //    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                //    smtp.Timeout = 20000;
                //}
                //// Passing values to smtp object
                ////smtp.Send(toAddress, toAddress, subject, body);
                //MailAddress from = new MailAddress(fromAddress);
                //MailAddress to = new MailAddress(toAddress);
                //MailMessage message = new MailMessage(from, to);
                //message.Body = body;
                //message.Subject = subject;
                //smtp.Send(message);

                var smtpdetails = SmtpSettings(toAddress, body, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In SendFeedbackEmail: " + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public void LimitExceededMail()
        {

            string userId = ConfigurationManager.AppSettings["mailUserID"];
            string password = ConfigurationManager.AppSettings["mailPassword"];
            string ccAddress = ConfigurationManager.AppSettings["mailCCAddress"];
            password = CommonLayer.EncryptDecrypt.EncryptDecryptDES.DecryptString(password);
            string toAddress = ConfigurationManager.AppSettings["mailToAddress"];
            //string division = System.Web.HttpContext.Current.Session["DatabaseSeLection"] != null
            //                  && System.Web.HttpContext.Current.Session["DatabaseSeLection"].ToString() == "DefaultConnection"
            //                  ? "Mumbai divison" : "India divison";
            var UserName = System.Web.HttpContext.Current.User.Identity.Name;
            string body = "Following User Has Exceeded The Limit On QA-Series Application.Kindly Deactive To The User." + ".\n \n " + "UserName : " + UserName + ".\n \n ";
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = ConfigurationManager.AppSettings["smtpHost"];
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPortNo"]);
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["sslSecurityStatus"]);
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                smtp.Credentials = new NetworkCredential(userId, password);
                smtp.Timeout = 20000;
            }

            MailAddress from = new MailAddress(userId);
            MailAddress to = new MailAddress(toAddress);
            MailAddress cc = new MailAddress(ccAddress);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Limit Exceeded Alert!";
            message.Body = body;
            message.CC.Add(cc);

            try
            {
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }

        }

        public int SmtpSettings(string toAddress,string body,string subject)
        {
            int Result=0;
            try
            {
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
                MailAddress to = new MailAddress(toAddress);
                MailMessage message = new MailMessage(from, to);
                message.Body = body;
                message.Subject = subject;
                smtp.Send(message);
                Result = 1;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error In SmtpSettings: " + ex.Message + Environment.NewLine + ex.StackTrace);
                Result = 0;
            }
            return Result;
        }


    }
}
