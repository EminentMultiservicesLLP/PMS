using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.API.Masters.Interface;
using PMS.Areas.Masters.Models;
using CommonDataLayer.DataAccess;
using System.Data;
using PMS.QueryCollection.Master;

namespace PMS.API.Masters.Repository
{
    public class EmployeeMasterRepository : IEmployeeMstInterface
    {
        //
        // GET: /EmployeeMasterRepository/
        public int CreateNewEmployeeMasters(EmployeeModel model)
        {
            int iResult;
            using (var dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("EmpId", model.EmpId, DbType.Int32, ParameterDirection.Output));
                paramCollection.Add(new DBParameter("FirstName", model.FirstName, DbType.String));
                paramCollection.Add(new DBParameter("LastName", model.LastName, DbType.String));
                paramCollection.Add(new DBParameter("DesignationId", model.DesignationId, DbType.Int32));
                paramCollection.Add(new DBParameter("JoiningDate", model.JoiningDate, DbType.DateTime));
                paramCollection.Add(new DBParameter("ConfirmationDate", model.ConfirmationDate, DbType.DateTime));
                paramCollection.Add(new DBParameter("LastPromoDate", model.LastPromoDate, DbType.DateTime));
                paramCollection.Add(new DBParameter("Salary", model.Salary, DbType.Double));
                paramCollection.Add(new DBParameter("DeptId", model.DeptId, DbType.Int32));
                paramCollection.Add(new DBParameter("RoleId", model.RoleId, DbType.Int32));
                paramCollection.Add(new DBParameter("QuestionnaireId", model.QuestionnaireId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserEmpId", model.AppraiserEmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserTwoEmpId", model.AppraiserTwoEmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                paramCollection.Add(new DBParameter("OutletId", model.OutletId, DbType.Int32));
                paramCollection.Add(new DBParameter("EmpCode", model.EmpCode, DbType.String));
                iResult = dbHelper.ExecuteNonQueryForOutParameter<int>(MasterQueries.CreateNewEmployeeMasters, paramCollection, CommandType.StoredProcedure, "EmpId");
            }
            return iResult;
        }

        public int CreateNewUserMaster(EmployeeModel model,int EmpId,bool IsExist)
        {
            int iResult;
            using (var dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                if (!IsExist)
                {
                    paramCollection.Add(new DBParameter("UserId", model.UserId, DbType.Int32, ParameterDirection.Output));
                    paramCollection.Add(new DBParameter("EmpId", model.EmpId, DbType.Int32));
                    paramCollection.Add(new DBParameter("LoginName", model.FirstName, DbType.String));
                    paramCollection.Add(new DBParameter("Password", model.EmpCode, DbType.String));
                    paramCollection.Add(new DBParameter("RoleId", model.RoleId, DbType.Int32));
                    paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
               
                    iResult = dbHelper.ExecuteNonQueryForOutParameter<int>(MasterQueries.CreateNewUserMasters, paramCollection, CommandType.StoredProcedure, "UserId");
                }
                else
                {
                    paramCollection.Add(new DBParameter("UserId", model.UserId, DbType.Int32));
                    paramCollection.Add(new DBParameter("EmpId", model.EmpId, DbType.Int32));
                    paramCollection.Add(new DBParameter("LoginName", model.FirstName, DbType.String));
                    paramCollection.Add(new DBParameter("Password", model.EmpCode , DbType.String));
                    paramCollection.Add(new DBParameter("RoleId", model.RoleId, DbType.Int32));
                    paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));

                    iResult = dbHelper.ExecuteNonQuery(MasterQueries.UpdateUserMasters, paramCollection, CommandType.StoredProcedure);
                }
                
            }
            return iResult;
        }
        public IEnumerable<EmployeeModel> GetAllEmployee(int UserId)
        {
            List<EmployeeModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            //  paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtdsg = dbHelper.ExecuteDataTable(MasterQueries.GetAllEmployee, CommandType.StoredProcedure);
                list = dtdsg.AsEnumerable()
                            .Select(row => new EmployeeModel
                            {
                                EmpId = row.Field<int>("EmpId"),
                                DesignationId = row.Field<int>("DesignationId"),
                                DeptId = row.Field<int>("DeptId"),
                                OutletId = row.Field<int>("OutletId"),
                                EmpCode = row.Field<string>("EmpCode"),
                                FirstName = row.Field<string>("FirstName"),
                                LastName = row.Field<string>("LastName"),
                                DeptName = row.Field<string>("DeptName"),
                                OutletName = row.Field<string>("OutletName"),
                                Salary = row.Field<double>("Salary"),
                                StrJoiningDate = row.Field<string>("JoiningDate"),
                                StrConfirmationDate = row.Field<string>("ConfirmationDate"),
                                StrLastPromoDate = row.Field<string>("LastPromoDate"),
                                AppraiserEmpId = row.Field<int>("AppraiserEmpId"),
                                AppraiserTwoEmpId = row.Field<int>("AppraiserTwoEmpId"),
                                DesignationName = row.Field<string>("DesignationName"),
                                QuestionnaireId = row.Field<int>("QuestionnaireId"),
                                RoleId = row.Field<int>("RoleId"),
                                Deactive = row.Field<bool>("Deactive"),
                            }).ToList();
            }
            return list;
        }
        public IEnumerable<EmployeeModel> GetAllActiveEmployee(int UserId)
        {
            List<EmployeeModel> list = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            //  paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtbatch = dbHelper.ExecuteDataTable(MasterQueries.GetAllActiveEmployee, CommandType.StoredProcedure);
                list = dtbatch.AsEnumerable()
                            .Select(row => new EmployeeModel
                            {
                                EmpId = row.Field<int>("EmpId")

                            }).ToList();
            }
            return list;


        }
        public bool UpdateEmployeeMasters(EmployeeModel model)
        {
            int iResult = 0;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("EmpId", model.EmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("FirstName", model.FirstName, DbType.String));
                paramCollection.Add(new DBParameter("LastName", model.LastName, DbType.String));
                paramCollection.Add(new DBParameter("JoiningDate", model.JoiningDate, DbType.DateTime));
                paramCollection.Add(new DBParameter("ConfirmationDate", model.ConfirmationDate, DbType.DateTime));
                paramCollection.Add(new DBParameter("LastPromoDate", model.LastPromoDate, DbType.DateTime));
                paramCollection.Add(new DBParameter("DesignationId", model.DesignationId, DbType.Int32));
                paramCollection.Add(new DBParameter("OutletId", model.OutletId, DbType.Int32));
                paramCollection.Add(new DBParameter("DeptId", model.DeptId, DbType.Int32));
                paramCollection.Add(new DBParameter("RoleId", model.RoleId, DbType.Int32));
                paramCollection.Add(new DBParameter("QuestionnaireId", model.QuestionnaireId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserEmpId", model.AppraiserEmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("AppraiserTwoEmpId", model.AppraiserTwoEmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("Salary", model.Salary, DbType.Double));
                paramCollection.Add(new DBParameter("EmpCode", model.EmpCode, DbType.String));
                paramCollection.Add(new DBParameter("Deactive", model.Deactive, DbType.Boolean));
                iResult = dbHelper.ExecuteNonQuery(MasterQueries.UpdateNewEmployeeMasters, paramCollection, CommandType.StoredProcedure);
            }
            if (iResult > 0)
                return true;
            else
                return false;
        }
        public bool CheckDuplicateItem( string callfrom,int EmpId,string EmpCode, string type)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                paramCollection.Add(new DBParameter("Type", type, DbType.String));
               
                paramCollection.Add(new DBParameter("EmpId", EmpId, DbType.Int32));
               
                paramCollection.Add(new DBParameter("EmpCode", EmpCode, DbType.String));
             
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                if(callfrom=="new")
                {
                    bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateItem, paramCollection, CommandType.StoredProcedure, "IsExist");
                }
                if (callfrom == "update")
                {
                    bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateUpdate, paramCollection, CommandType.StoredProcedure, "IsExist");
                }

            }
            return bResult;
        }
        public IEnumerable<EmployeeModel> GetAllRole()
        {
            List<EmployeeModel> list = null;
          
            //  paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
           
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtdsg = dbHelper.ExecuteDataTable(MasterQueries.GetAllRole, CommandType.StoredProcedure);
                list = dtdsg.AsEnumerable()
                            .Select(row => new EmployeeModel
                            {

                                RoleName = row.Field<string>("RoleName"),
                                RoleId = row.Field<int>("RoleId"),


                            }).ToList();
            }
            return list;
        }
        
        public bool CheckDuplicateUser(int EmpId)
        {
            bool bResult = false;
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                              
                              
                paramCollection.Add(new DBParameter("EmpId", EmpId, DbType.Int32));

                //  paramCollection.Add(new DBParameter("ID", typeid, DbType.String));
                paramCollection.Add(new DBParameter("IsExist", true, DbType.Boolean, ParameterDirection.Output));

                bResult = dbHelper.ExecuteNonQueryForOutParameter<bool>(MasterQueries.CheckDuplicateUser, paramCollection, CommandType.StoredProcedure, "IsExist");
            }
            return bResult;
        }

        public void DeactivateUser(int EmpId,int RoleId)
        {
            using (DBHelper dbHelper = new DBHelper())
            {
                DBParameterCollection paramCollection = new DBParameterCollection();
                
                paramCollection.Add(new DBParameter("EmpId", EmpId, DbType.Int32));
                paramCollection.Add(new DBParameter("RoleId", RoleId, DbType.Int32));

                dbHelper.ExecuteNonQuery(MasterQueries.DeactivateUser, paramCollection, CommandType.StoredProcedure);
            }

        }



    }
}
