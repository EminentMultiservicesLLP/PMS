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
    public class GradeMasterRepository : IGradeInterface
    {
        //
        // GET: /GradeMasterRepository/
        public int CreateNewGradeMasters(GradeModel model)
        {
            int iResult;
            using (var dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("GradeId", model.GradeId, DbType.Int32, ParameterDirection.Output));
                paramCollection.Add(new DBParameter("GradeName", model.GradeName, DbType.String));
                paramCollection.Add(new DBParameter("GradePoints", model.GradePoints, DbType.String));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQueryForOutParameter<int>(MasterQueries.CreateNewGradeMasters, paramCollection, CommandType.StoredProcedure, "GradeId");
            }
            return iResult;
        }
        public bool UpdateGradeMasters(GradeModel model)
        {
            int iResult = 0;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("GradeId", model.GradeId, DbType.Int32));
                paramCollection.Add(new DBParameter("GradePoints", model.GradePoints, DbType.String));
                paramCollection.Add(new DBParameter("GradeName", model.GradeName, DbType.String));

                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQuery(MasterQueries.UpdateNewGradeMasters, paramCollection, CommandType.StoredProcedure);
            }
            if (iResult > 0)
                return true;
            else
                return false;
        }
        public IEnumerable<GradeModel> GetAllGrade(int UserId)
        {
            List<GradeModel> list = null;
            // DBParameterCollection paramCollection = new DBParameterCollection();
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtdsg = dbHelper.ExecuteDataTable(MasterQueries.GetAllGradeMst, CommandType.StoredProcedure);
                list = dtdsg.AsEnumerable()
                            .Select(row => new GradeModel
                            {
                                GradeId = row.Field<int>("GradeId"),
                                GradeName = row.Field<string>("GradeName"),
                                GradePoints = row.Field<int>("GradePoints"),
                                Deactive = row.Field<bool>("Deactive"),
                            }).ToList();
            }
            return list;
        }
        public IEnumerable<GradeModel> GetAllActiveGrade(int UserId)
        {
            List<GradeModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtbatch = dbHelper.ExecuteDataTable(MasterQueries.GetAllActiveGrade, CommandType.StoredProcedure);
                list = dtbatch.AsEnumerable()
                            .Select(row => new GradeModel
                            {
                                GradeId = row.Field<int>("GradeId"),

                                GradeName = row.Field<string>("GradeName"),
                            }).ToList();
            }
            return list;
        }
        //
        // GET: /DepartmentMasterRepository/
        public bool CheckDuplicateItem(string GradeName, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));
                paramCollection.Add(new DBParameter("GradeName", GradeName, DbType.String));
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));
                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateItem, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }

        public bool CheckDuplicateUpdate(GradeModel model, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));

                paramCollection.Add(new DBParameter("GradeId", model.GradeId, DbType.String));

                paramCollection.Add(new DBParameter("GradeName", model.GradeName, DbType.String));

                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateUpdate, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }




    }
}
