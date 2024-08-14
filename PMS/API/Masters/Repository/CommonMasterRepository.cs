using PMS.API.Masters.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Areas.Masters.Models;
using CommonDataLayer.DataAccess;
using System.Data;
using PMS.QueryCollection.Master;

namespace PMS.API.Masters.Repository
{
    public class CommonMasterRepository : ICommonMaster
    {
        public List<CommonMasterModel> GetAllDesignation()
        {
            List<CommonMasterModel> list = null;
            
                using (DBHelper dbHelper = new DBHelper())
                {
                    DataTable dtPaper = dbHelper.ExecuteDataTable(MasterQueries.GetAllDesignation, CommandType.Text);
                    list = dtPaper.AsEnumerable()
                        .Select(row => new CommonMasterModel
                        {
                            DesignationId = row.Field<int>("DesignationId"),
                            DesignationName = row.Field<string>("DesignationName"),
                            Deactive = row.Field<bool>("Deactive")
                        }).ToList();
                }            
            

            return list;
        }


        public List<CommonMasterModel> GetAllGrade()
        {
            List<CommonMasterModel> list = null;

            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtPaper = dbHelper.ExecuteDataTable(MasterQueries.GetAllGrade, CommandType.Text);
                list = dtPaper.AsEnumerable()
                    .Select(row => new CommonMasterModel
                    {
                        GradeId = row.Field<int>("GradeId"),
                        GradeName = row.Field<string>("GradeName"),
                        GradePoints = row.Field<int>("GradePoints"),
                        Deactive = row.Field<bool>("Deactive")
                    }).ToList();
            }


            return list;

        }

        public List<CommonMasterModel> GetAllOutlet()
        {
            List<CommonMasterModel> list = null;

            using (DBHelper dbHelper = new DBHelper())
            {
                DataTable dtPaper = dbHelper.ExecuteDataTable(MasterQueries.GetAllOutlet, CommandType.Text);
                list = dtPaper.AsEnumerable()
                    .Select(row => new CommonMasterModel
                    {
                        OutletId = row.Field<int>("OutletId"),
                        OutletName = row.Field<string>("OutletName"),
                        Deactive = row.Field<bool>("Deactive")
                    }).ToList();
            }


            return list;
        }

        

    }
}