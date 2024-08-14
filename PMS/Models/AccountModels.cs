using CommonDataLayer.DataAccess;
using CommonLayer;
using CommonLayer.EncryptDecrypt;
using CommonLayer.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Security;

namespace PMS.Models
{
    public class UsersContext : DbContext
    {
       
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        private static readonly ILogger Logger = CommonLayer.Logger.Register(typeof(LoginModel));
        public int UserID { get; set; }
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please Provide Username", AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please provide password", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public int RoleId { get; set; }

       // public bool IsLogin { get; set; }

        public string SessionId { get; set; }

        public string LoginName { get; set; }
        public bool IsDeactive { get; set; }
     


        /// <summary>
        /// Checks if user with given password exists in the database
        /// </summary>
        /// <param name="_username">User name</param>
        /// <param name="_password">User password</param>
        /// <returns>True if user exist and password is correct</returns>
       

        public LoginModel GetUserIdNew(string _username, string _password)
        {
            LoginModel model = new LoginModel();
            TryCatch.Run(() =>
            {
                int UserId = 0;
                var SessionId = System.Web.HttpContext.Current.Session.SessionID;
                //_password = EncryptDecryptDES.EncryptString(_password);
                using (DBHelper Dbhelper = new DBHelper())
                {
                    DBParameterCollection paramCollection = new DBParameterCollection();
                    paramCollection.Add(new DBParameter("Username", _username, DbType.String));
                    paramCollection.Add(new DBParameter("Password", _password, DbType.String));
                    DataTable dtUser = Dbhelper.ExecuteDataTable("dbsp_ValidateUser", paramCollection, CommandType.StoredProcedure);
                    if (dtUser.Rows.Count > 0)
                    {
                        model.UserID = Convert.ToInt32(dtUser.Rows[0]["UserID"].ToString());
                        //SetSessionId(model.UserID, SessionId);
                        model.Username = dtUser.Rows[0]["UserName"].ToString();
                        model.LoginName = dtUser.Rows[0]["LoginName"].ToString();
                       //model.Password = EncryptDecryptDES.DecryptString(_password);
                        if (dtUser.Rows[0]["RoleId"].ToString() != "")
                        {
                            model.RoleId = Convert.ToInt32(dtUser.Rows[0]["RoleId"].ToString());
                        }
                        model.IsDeactive = Convert.ToBoolean(dtUser.Rows[0]["IsDeactive"]);
                       
                    }
                }               
            }).IfNotNull((ex) =>
            {
                Logger.LogError("Error in Account Model  GetUserId:" + ex.Message + Environment.NewLine + ex.StackTrace);
            });

            return model;
        }


        public static string ConnectionString
        {
            get
            {
                // return "Server=192.168.2.212;Database=BISNowERP;User Id=sa;Password=optimal$2009;";
                return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }

        //public void SetUserIsLogin(int userid, bool Status,string SessionId)
        //{
        //    DBParameterCollection paramCollection = new DBParameterCollection();
        //    paramCollection.Add(new DBParameter("UserId", userid, DbType.Int32));
        //    paramCollection.Add(new DBParameter("Status", Status, DbType.Boolean));
        //    paramCollection.Add(new DBParameter("SessionId", SessionId, DbType.String));

        //    using (DBHelper dbHelper = new DBHelper())
        //    {
        //        dbHelper.ExecuteNonQuery("dbsp_SetUserIsLogin", paramCollection, CommandType.StoredProcedure);
        //    }

        //}

        public void SetSessionId(int userid, string SessionId)
        {
            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("UserId", userid, DbType.Int32));            
            paramCollection.Add(new DBParameter("SessionId", SessionId, DbType.String));

            using (DBHelper dbHelper = new DBHelper())
            {
                dbHelper.ExecuteNonQuery("dbsp_SetUserSessionId", paramCollection, CommandType.StoredProcedure);
            }
        }

        public bool CheckSession(int Userid)
        {
            bool IsSessionActive; object UserSessionId;
            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("UserId", Userid, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                UserSessionId = dbHelper.ExecuteScalar("dbsp_CheckSessionId", paramCollection, CommandType.StoredProcedure);
            }
            
                if (Convert.ToString(System.Web.HttpContext.Current.Session.SessionID) == Convert.ToString(UserSessionId) )
                IsSessionActive = true;
                else 
                IsSessionActive = false;
            return IsSessionActive;
        }

    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
