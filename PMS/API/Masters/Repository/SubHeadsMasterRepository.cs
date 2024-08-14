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
    public class SubHeadsMasterRepository : ISubHeadsInterface
    {
        //
        // GET: /SubHeadsMasterRepository/

        public int CreateNewSubHeadsMasters(SubheadsModel model)
        {
            int iResult;
            using (var dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("SubHeadId", model.HeadId, DbType.Int32, ParameterDirection.Output));
                paramCollection.Add(new DBParameter("SubHeadName", model.SubHeadName, DbType.String));
                paramCollection.Add(new DBParameter("HeadId", model.HeadId, DbType.Int32));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQueryForOutParameter<int>(MasterQueries.CreateNewSubHeadsMasters, paramCollection, CommandType.StoredProcedure, "SubHeadId");
            }
            return iResult;
        }
        public bool CheckDuplicateItem(string SubHeadName, int HeadId, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));
                paramCollection.Add(new DBParameter("SubHeadName", SubHeadName, DbType.String));
                paramCollection.Add(new DBParameter("HeadId", HeadId, DbType.Int32));

                //  paramCollection.Add(new DBParameter("ID", typeid, DbType.String));
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateItem, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }
        public bool UpdateSubHeadsMasters(SubheadsModel model)
        {
            int iResult = 0;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("SubHeadId", model.SubHeadId, DbType.Int32));
                paramCollection.Add(new DBParameter("SubHeadName", model.SubHeadName, DbType.String));
                paramCollection.Add(new DBParameter("HeadId", model.HeadId, DbType.Int32));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQuery(MasterQueries.UpdateNewSubHeadsMasters, paramCollection, CommandType.StoredProcedure);
            }
            if (iResult > 0)
                return true;
            else
                return false;
        }
        public IEnumerable<SubheadsModel> GetAllSubHeads(int UserId)
        {
            List<SubheadsModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtdsg = dbHelper.ExecuteDataTable(MasterQueries.GetAllSubHeadsMst, CommandType.StoredProcedure);
                list = dtdsg.AsEnumerable()
                            .Select(row => new SubheadsModel
                            {
                                SubHeadId = row.Field<int>("SubHeadId"),
                                //BatchCode = row.Field<string>("BatchCode"),
                                HeadId = row.Field<int>("HeadId"),
                                HeadName = row.Field<string>("HeadName"),
                                QuestionnaireId = row.Field<int>("QuestionnaireId"),
                                QuestionnaireName = row.Field<string>("QuestionnaireName"),
                                SubHeadName = row.Field<string>("SubHeadName"),
                                Deactive = row.Field<bool>("Deactive"),
                            }).ToList();
            }
            return list;
        }
        public IEnumerable<SubheadsModel> GetAllActiveSubHeads(int UserId)
        {
            List<SubheadsModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            //  paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtbatch = dbHelper.ExecuteDataTable(MasterQueries.GetAllActiveSubHeads, CommandType.StoredProcedure);
                list = dtbatch.AsEnumerable()
                            .Select(row => new SubheadsModel
                            {
                                HeadId = row.Field<int>("HeadId"),
                                HeadName = row.Field<string>("HeadName"),
                            }).ToList();
            }
            return list;
        }
        public IEnumerable<SubheadsModel> GetAllHeadName(int QuestionnaireId)
        {
            List<SubheadsModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
          //  paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            paramCollection.Add(new DBParameter("QuestionnaireId", QuestionnaireId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtdsg = dbHelper.ExecuteDataTable(MasterQueries.GetAllHeadNmae, paramCollection,CommandType.StoredProcedure);
                list = dtdsg.AsEnumerable()
                            .Select(row => new SubheadsModel
                            {
                               
                                HeadName = row.Field<string>("HeadName"),
                                HeadId = row.Field<int>("HeadId"),
                                QuestionnaireName = row.Field<string>("QuestionnaireName"),

                            }).ToList();
            }
            return list;
        }
        public bool CheckDuplicateUpdate(string SubHeadName, int HeadId, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));
                paramCollection.Add(new DBParameter("HeadId", HeadId, DbType.Int32));
                paramCollection.Add(new DBParameter("SubHeadName", SubHeadName, DbType.String));
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));
                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateUpdate, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }

    }
}
