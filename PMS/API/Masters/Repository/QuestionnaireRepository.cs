using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PMS.API.Masters.Interface;
using PMS.Areas.Masters.Models;
using CommonDataLayer.DataAccess;
using System.Data;
using PMS.QueryCollection.Master;

namespace PMS.API.Masters.Repository
{
    public class QuestionnaireRepository:IQuestionnaireInterface
    {
       public int CreateNewQuestnMasters(QuestionnaireModel model)
        {
            int iResult;
            using (var dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("QuestionnaireId", model.QuestionnaireId, DbType.Int32, ParameterDirection.Output));
                paramCollection.Add(new DBParameter("QuestionnaireName", model.QuestionnaireName, DbType.String));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQueryForOutParameter<int>(MasterQueries.CreateNewQuestnMasters, paramCollection, CommandType.StoredProcedure, "QuestionnaireId");
            }
            return iResult;
        }

        public IEnumerable<QuestionnaireModel> GetAllQuestionnaire()
        {
            List<QuestionnaireModel> list = null;
           // DBParameterCollection paramCollection = new DBParameterCollection();            
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtdsg = dbHelper.ExecuteDataTable(MasterQueries.GetAllQuestnMst, CommandType.StoredProcedure);
                list = dtdsg.AsEnumerable()
                            .Select(row => new QuestionnaireModel
                            {
                                QuestionnaireId = row.Field<int>("QuestionnaireId"),
                                //BatchCode = row.Field<string>("BatchCode"),
                                QuestionnaireName = row.Field<string>("QuestionnaireName"),
                                Deactive = row.Field<bool>("Deactive"),
                            }).ToList();
            }
            return list;
        }

        public IEnumerable<QuestionnaireModel> GetAllActiveQuestion()
        {
            List<QuestionnaireModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtbatch = dbHelper.ExecuteDataTable(MasterQueries.GetAllActiveQuestn, CommandType.StoredProcedure);
                list = dtbatch.AsEnumerable()
                            .Select(row => new QuestionnaireModel
                            {
                                QuestionnaireId = row.Field<int>("QuestionnaireId"),
                                QuestionnaireName = row.Field<string>("QuestionnaireName"),
                            }).ToList();
            }
            return list;
        }


        public bool CheckDuplicateItem(string QuestionnaireName, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));
                paramCollection.Add(new DBParameter("QuestionnaireName", QuestionnaireName, DbType.String));
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateItem, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;

        }

        public bool CheckDuplicateUpdate(QuestionnaireModel model, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));

                paramCollection.Add(new DBParameter("QuestionnaireId", model.QuestionnaireId, DbType.String));

                paramCollection.Add(new DBParameter("QuestionnaireName", model.QuestionnaireName, DbType.String));

                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateUpdate, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }

        public bool UpdateQuestnMasters(QuestionnaireModel model)
        {
            int iResult = 0;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("QuestionnaireId", model.QuestionnaireId, DbType.Int32));
                paramCollection.Add(new DBParameter("QuestionnaireName", model.QuestionnaireName, DbType.String));

                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQuery(MasterQueries.UpdateNewQuestnMasters, paramCollection, CommandType.StoredProcedure);
            }
            if (iResult > 0)
                return true;
            else
                return false;
        }
    }
}