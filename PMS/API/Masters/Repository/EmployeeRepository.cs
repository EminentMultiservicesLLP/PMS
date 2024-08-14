using CommonDataLayer.DataAccess;
using PMS.API.Masters.Interface;
using PMS.Areas.Masters.Models;
using PMS.QueryCollection.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PMS.API.Masters.Repository
{
    public class EmployeeRepository : IEmployee
    {
        public List<EmployeeModel> GetEmpForReview(int UserId)
        {
            List <EmployeeModel> Employees = null;
            DBParameterCollection paramCollection = new DBParameterCollection();
            paramCollection.Add(new DBParameter("UserId", UserId, DbType.Int32));
            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtTable = dbHelper.ExecuteDataTable(MasterQueries.GetEmpForReview, paramCollection, CommandType.StoredProcedure);
                Employees = dtTable.AsEnumerable()
                    .Select(row => new EmployeeModel
                    {
                        EmpId = row.Field<int>("EmpId"),
                        EmpName = row.Field<string>("FirstName") +' '+ row.Field<string>("LastName"),
                        FirstName = row.Field<string>("FirstName"),
                        LastName = row.Field<string>("LastName"),
                        DesignationId = row.Field<int>("DesignationId"),
                        StrJoiningDate = row.Field<string>("JoiningDate"),
                        StrConfirmationDate = row.Field<string>("ConfirmationDate"),
                        StrLastPromoDate = row.Field<string>("LastPromoDate"),
                        Salary = row.Field<double>("Salary"),
                        DeptId = row.Field<int>("DeptId"),
                        AppraiserEmpId = row.Field<int>("AppraiserEmpId"),
                        AppraiserTwoEmpId = row.Field<int>("AppraiserTwoEmpId"),
                        DesignationName = row.Field<string>("DesignationName"),
                        AppraiserDesigName = row.Field<string>("AppraiserDesigName"),
                        AppraiserName = row.Field<string>("AppraiserName"),
                        OutletName = row.Field<string>("OutletName"),
                        EmpCode =row.Field<string>("EmpCode")
                    }).ToList();
            }

            return Employees;
        }

    }
}