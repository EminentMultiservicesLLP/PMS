
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using PMS.Areas.Masters.Models;
using CommonDataLayer.DataAccess;
using PMS.QueryCollection.Master;
using PMS.API.Masters.Interface;

namespace PMS.API.Masters.Repository
{
    public class DepartmentMasterRepository : IDepartmentInterface
    {
        public int CreateNewDepartmentMasters(DepartmentModel model)
        {
            int iResult;
            using (var dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("DeptId", model.DeptId, DbType.Int32, ParameterDirection.Output));
                paramCollection.Add(new DBParameter("DeptName", model.DeptName, DbType.String));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQueryForOutParameter<int>(MasterQueries.CreateNewDepartmentMasters, paramCollection, CommandType.StoredProcedure, "DeptId");
            }
            return iResult;
        }
        public bool UpdateDepartmentMasters(DepartmentModel model)
        {
            int iResult = 0;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("DeptId", model.DeptId, DbType.Int32));
                paramCollection.Add(new DBParameter("DeptName", model.DeptName, DbType.String));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQuery(MasterQueries.UpdateNewDepartmentMasters, paramCollection, CommandType.StoredProcedure);
            }
            if (iResult > 0)
                return true;
            else
                return false;
        }
        public IEnumerable<DepartmentModel> GetAllDepartment(int UserId)
        {
            List<DepartmentModel> list = null;
           // DBParameterCollection paramCollection = new DBParameterCollection();
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtdsg = dbHelper.ExecuteDataTable(MasterQueries.GetAllDepartmentMst, CommandType.StoredProcedure);
                list = dtdsg.AsEnumerable()
                            .Select(row => new DepartmentModel
                            {
                                DeptId = row.Field<int>("DeptId"),
                                DeptName = row.Field<string>("DeptName"),
                                Deactive = row.Field<bool>("Deactive"),
                            }).ToList();
            }
            return list;
        }
        public IEnumerable<DepartmentModel> GetAllActiveDepartment(int UserId)
        {
            List<DepartmentModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtbatch = dbHelper.ExecuteDataTable(MasterQueries.GetAllActiveDepartment, CommandType.StoredProcedure);
                list = dtbatch.AsEnumerable()
                            .Select(row => new DepartmentModel
                            {
                                DeptId = row.Field<int>("DeptId"),
                               
                                DeptName = row.Field<string>("DeptName"),
                            }).ToList();
            }
            return list;
        }
        //
        // GET: /DepartmentMasterRepository/
        public bool CheckDuplicateItem(string DeptName, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));
                paramCollection.Add(new DBParameter("DeptName", DeptName, DbType.String));
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));
                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateItem, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }

        public bool CheckDuplicateUpdate(DepartmentModel model, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));

                paramCollection.Add(new DBParameter("DeptId", model.DeptId, DbType.String));

                paramCollection.Add(new DBParameter("DeptName", model.DeptName, DbType.String));

                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateUpdate, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }

    }
}
