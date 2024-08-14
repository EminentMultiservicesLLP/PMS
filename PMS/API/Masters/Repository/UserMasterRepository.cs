using CommonDataLayer.DataAccess;
using PMS.API.Masters.Interface;
using PMS.Areas.Masters.Models;
using PMS.QueryCollection.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.API.Masters.Repository
{
    public class UserMasterRepository : IUserInterface
    {
        //
        // GET: /UserMasterRepository/
        public IEnumerable<UserModel> GetAllUser()
        {
            List<UserModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            // paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtdsg = dbHelper.ExecuteDataTable(MasterQueries.GetAllUserMst, CommandType.StoredProcedure);
                list = dtdsg.AsEnumerable()
                            .Select(row => new UserModel
                            {
                                UserId = row.Field<int>("UserID"),
                                //BatchCode = row.Field<string>("BatchCode"),
                                EmpId = row.Field<int>("EmpId"),
                                RoleId=row.Field<int>("RoleID"),
                                EmpCode = row.Field<string>("EmpCode"),
                                LoginName = row.Field<string>("LoginName"),
                                LastName = row.Field<string>("LastName"),
                                FirstName = row.Field<string>("FirstName"),
                                Password = row.Field<string>("Password"),
                                Deactive = row.Field<bool>("Deactive"),
                            }).ToList();
            }
            return list;
        }
        public IEnumerable<UserModel> GetAllActiveHeads()
        {
            List<UserModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            //  paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtbatch = dbHelper.ExecuteDataTable(MasterQueries.GetAllActiveUser, CommandType.StoredProcedure);
                list = dtbatch.AsEnumerable()
                            .Select(row => new UserModel
                            {
                                UserId = row.Field<int>("UserId"),
                                LoginName = row.Field<string>("LoginName"),
                            }).ToList();
            }
            return list;
        }

        public bool UpdateUserMasters(UserModel model)
        {
            int iResult = 0;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("UserID", model.UserId, DbType.Int32));
                
                paramCollection.Add(new DBParameter("Password", model.Password, DbType.String));
                //paramCollection.Add(new DBParameter("RoleID", model.RoleId, DbType.Int32));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQuery(MasterQueries.UpdateNewUserMasters, paramCollection, CommandType.StoredProcedure);
            }
            if (iResult > 0)
                return true;
            else
                return false;
        }

        public bool CheckDuplicateUpdate(int UserId,int EmpId, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));
                paramCollection.Add(new DBParameter("EmpId", EmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
                //paramCollection.Add(new DBParameter("Password",Password, DbType.String));
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));
                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateUpdate, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }
    }
}
