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
    public class OutletMasterRepository : IOutletInterface
    {
        //
        // GET: /OutletMasterRepository/

        public int CreateNewOutletMasters(OutletModel model)
        {
            int iResult;
            using (var dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("OutletId", model.OutletId, DbType.Int32, ParameterDirection.Output));
                paramCollection.Add(new DBParameter("OutletName", model.OutletName, DbType.String));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQueryForOutParameter<int>(MasterQueries.CreateNewOutletMasters, paramCollection, CommandType.StoredProcedure, "OutletId");
            }
            return iResult;
        }
        public bool UpdateOutletMasters(OutletModel model)
        {
            int iResult = 0;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("OutletId", model.OutletId, DbType.Int32));
                paramCollection.Add(new DBParameter("OutletName", model.OutletName, DbType.String));

                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQuery(MasterQueries.UpdateNewOutletMasters, paramCollection, CommandType.StoredProcedure);
            }
            if (iResult > 0)
                return true;
            else
                return false;
        }
        public IEnumerable<OutletModel> GetAllOutlet(int UserId)
        {
            List<OutletModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtdsg = dbHelper.ExecuteDataTable(MasterQueries.GetAllOutletMst, CommandType.StoredProcedure);
                list = dtdsg.AsEnumerable()
                            .Select(row => new OutletModel
                            {
                                OutletId = row.Field<int>("OutletId"),
                                //BatchCode = row.Field<string>("BatchCode"),
                                OutletName = row.Field<string>("OutletName"),
                                Deactive = row.Field<bool>("Deactive"),
                            }).ToList();
            }
            return list;
        }
        public IEnumerable<OutletModel> GetAllActiveOutlet(int UserId)
        {
            List<OutletModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtbatch = dbHelper.ExecuteDataTable(MasterQueries.GetAllActiveOutlet, CommandType.StoredProcedure);
                list = dtbatch.AsEnumerable()
                            .Select(row => new OutletModel
                            {
                                OutletId = row.Field<int>("OutletId"),
                                OutletName = row.Field<string>("OutletName"),
                            }).ToList();
            }
            return list;
        }
        //
        // GET: /OutletMasterRepository/
        public bool CheckDuplicateItem(string OutletName, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));
                paramCollection.Add(new DBParameter("OutletName", OutletName, DbType.String));
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateItem, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }

        public bool CheckDuplicateUpdate(OutletModel model, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));

                paramCollection.Add(new DBParameter("OutletId", model.OutletId, DbType.String));

                paramCollection.Add(new DBParameter("OutletName", model.OutletName, DbType.String));

                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateUpdate, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }


    }
}
