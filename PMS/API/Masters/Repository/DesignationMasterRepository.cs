using PMS.API.Masters.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using PMS.Areas.Masters.Models;
using CommonDataLayer.DataAccess;
using PMS.QueryCollection.Master;

namespace PMS.API.Masters.Repository
{
    public class DesignationMasterRepository : IDesignationInterface
    {
        public int CreateNewDesignationMasters(DesignationModel model)
        {
            int iResult;
            using (var dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("DesignationId", model.DesignationId, DbType.Int32, ParameterDirection.Output));
                //paramCollection.Add(new DBParameter("BatchCode", model.BatchCode, DbType.String));
                paramCollection.Add(new DBParameter("DesignationName", model.DesignationName, DbType.String));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQueryForOutParameter<int>(MasterQueries.CreateNewDesignationMasters, paramCollection, CommandType.StoredProcedure,"DesignationId");
            }
            return iResult;
        }
        public bool UpdateDesignationMasters(DesignationModel model)
        {
            int iResult = 0;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("DesignationId", model.DesignationId, DbType.Int32));
                //paramCollection.Add(new DBParameter("BatchCode", model.BatchCode, DbType.String));
                paramCollection.Add(new DBParameter("DesignationName", model.DesignationName, DbType.String));
               
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQuery(MasterQueries.UpdateNewDesignationMasters, paramCollection, CommandType.StoredProcedure);
            }
            if (iResult > 0)
                return true;
            else
                return false;
        }
        public IEnumerable<DesignationModel> GetAllDesignation(int UserId)
        {
            List<DesignationModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtdsg = dbHelper.ExecuteDataTable(MasterQueries.GetAllDesignationMst, CommandType.StoredProcedure);
                list = dtdsg.AsEnumerable()
                            .Select(row => new DesignationModel
                            {
                                DesignationId = row.Field<int>("DesignationId"),
                                //BatchCode = row.Field<string>("BatchCode"),
                                DesignationName = row.Field<string>("DesignationName"),
                                Deactive = row.Field<bool>("Deactive"),
                            }).ToList();
            }
            return list;
        }
        public IEnumerable<DesignationModel> GetAllActiveDesignation(int UserId)
        {
            List<DesignationModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
          //  paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtbatch = dbHelper.ExecuteDataTable(MasterQueries.GetAllActiveDesignation,CommandType.StoredProcedure);
                list = dtbatch.AsEnumerable()
                            .Select(row => new DesignationModel
                            {
                                DesignationId = row.Field<int>("DesignationId"),
                               // BatchCode = row.Field<string>("BatchCode"),
                                DesignationName = row.Field<string>("DesignationName"),
                            }).ToList();
            }
            return list;
        }
        //
        // GET: /DesignationMasterRepository/
        public bool CheckDuplicateItem(string DesignationName,string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));
                paramCollection.Add(new DBParameter("DesignationName", DesignationName, DbType.String));
               //  paramCollection.Add(new DBParameter("ID", typeid, DbType.String));
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateItem, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }

        public bool CheckDuplicateUpdate(DesignationModel model,string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));

                paramCollection.Add(new DBParameter("DesignationId", model.DesignationId, DbType.String));

                paramCollection.Add(new DBParameter("DesignationName", model.DesignationName, DbType.String));
              
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateUpdate, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }

    }
}
