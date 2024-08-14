using CommonLayer.EncryptDecrypt;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class LoginPageModel
    {
        public string Username { get; set; }
        //  public string LoginName { get; set; }
        public string OldPassword { get; set; }
        
        public int UserID { get; set; }
        public string NewPassword { get; set; }
        //public int? InsertedBy { get; set; }
        //public DateTime? InsertedOn { get; set; }
        //public string InsertedMacName { get; set; }
        //public string InsertedMacId { get; set; }
        //public string InsertedIpAddress { get; set; }
        public string Message { get; set; }

        public int GetUserId(string _username, string _password)
        {
            int UserId = 0;
            using (var cn = new SqlConnection(ConnectionString))
            {
                _password = EncryptDecryptDES.EncryptString(_password);
                string _sql = @"SELECT * FROM [dbo].[Um_Mst_User] WHERE [LoginName] = @u And [Password] = @p and isnull(isdeactive,0)=0";
                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = _username;
                cmd.Parameters
                    .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                    .Value = _password;
                //cmd.Parameters
                //    .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                //    .Value = Helpers.SHA1.Encode(_password);
                cn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserId = Convert.ToInt32(reader["UserID"].ToString());
                        break;
                    }
                    reader.Dispose();
                    cmd.Dispose();
                    return UserId;
                }
                else
                {
                    reader.Dispose();
                    cmd.Dispose();
                    return UserId;
                }
            }
            //return UserId;
        }
        public static string ConnectionString
        {
            get
            {
                // return "Server=192.168.2.212;Database=BISNowERP;User Id=sa;Password=optimal$2009;";
                return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }
    }
}