
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CommonDataLayer.DataAccess;
using CommonLayer;
using PMS.QueryCollection.AdminPanel;
using PMS.API.LoginPanel.Interface;
using PMS.Models;
using CommonLayer.EncryptDecrypt;

namespace PMS.API.LoginPanel.Repository
{
    public class LoginPageRepository : ILoginPageRepository
    {


        public int SaveLoginPage(LoginPageModel model)
        { 
         int iResult = 0;
            using (DBHelper dbHelper = new DBHelper())
            {
                    DBParameterCollection paramCollection = new DBParameterCollection();
                    paramCollection.Add(new DBParameter("Username", model.Username, DbType.String));
                    paramCollection.Add(new DBParameter("UserID", model.UserID, DbType.String));
                    paramCollection.Add(new DBParameter("LoginName", model.Username, DbType.String));
                    paramCollection.Add(new DBParameter("NewPassword", EncryptDecryptDES.EncryptString(model.NewPassword), DbType.String));
                    iResult = dbHelper.ExecuteNonQuery(AdminPanelQueries.SaveLoginPage, paramCollection, CommandType.StoredProcedure);
                }
                    return iResult;
                }

            }
        }
    