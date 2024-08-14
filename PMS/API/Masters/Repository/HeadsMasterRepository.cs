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
    public class HeadsMasterRepository : IHeadsInterface
    {
        //
        // GET: /HeadsMasterRepository/
        public int CreateNewHeadsMasters(HeadsModel model)
        {
            int iResult;
            using (var dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("HeadId", model.HeadId, DbType.Int32, ParameterDirection.Output));
                paramCollection.Add(new DBParameter("HeadName", model.HeadName, DbType.String));
                paramCollection.Add(new DBParameter("QuestionnaireId", model.QuestionnaireId, DbType.Int32));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQueryForOutParameter<int>(MasterQueries.CreateNewHeadsMasters, paramCollection, CommandType.StoredProcedure, "HeadId");
            }
            return iResult;
        }

        public IEnumerable<HeadsModel> GetAllHeads()
        {
            List<HeadsModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
           // paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtdsg = dbHelper.ExecuteDataTable(MasterQueries.GetAllHeadsMst, CommandType.StoredProcedure);
                list = dtdsg.AsEnumerable()
                            .Select(row => new HeadsModel
                            {
                                HeadId = row.Field<int>("HeadId"),
                                //BatchCode = row.Field<string>("BatchCode"),
                                QuestionnaireId = row.Field<int>("QuestionnaireId"),
                                QuestionnaireName = row.Field<string>("QuestionnaireName"),
                                HeadName = row.Field<string>("HeadName"),
                                Deactive = row.Field<bool>("Deactive"),
                            }).ToList();
            }
            return list;
        }
        public IEnumerable<HeadsModel> GetAllActiveHeads()
        {
            List<HeadsModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            //  paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtbatch = dbHelper.ExecuteDataTable(MasterQueries.GetAllActiveHeads, CommandType.StoredProcedure);
                list = dtbatch.AsEnumerable()
                            .Select(row => new HeadsModel
                            {
                                HeadId = row.Field<int>("HeadId"),
                                HeadName = row.Field<string>("HeadName"),
                            }).ToList();
            }
            return list;
        }
        public bool UpdateHeadsMasters(HeadsModel model)
        {
            int iResult = 0;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("HeadId", model.HeadId, DbType.Int32));
                paramCollection.Add(new DBParameter("HeadName", model.HeadName, DbType.String));
                paramCollection.Add(new DBParameter("QuestionnaireId", model.QuestionnaireId, DbType.Int32));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQuery(MasterQueries.UpdateNewHeadsMasters, paramCollection, CommandType.StoredProcedure);
            }
            if (iResult > 0)
                return true;
            else
                return false;
        }
        public bool CheckDuplicateItem(string HeadName,int QuestionnaireId, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));
                paramCollection.Add(new DBParameter("HeadName", HeadName, DbType.String));
                paramCollection.Add(new DBParameter("QuestionnaireId", QuestionnaireId, DbType.Int32));

                //  paramCollection.Add(new DBParameter("ID", typeid, DbType.String));
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateItem, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }
        public bool CheckDuplicateUpdate(string HeadName, int DeptId, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));
                paramCollection.Add(new DBParameter("DeptId", DeptId, DbType.Int32));
                paramCollection.Add(new DBParameter("HeadName", HeadName, DbType.String));
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));
                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateUpdate, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }

    }
}
