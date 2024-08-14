using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using PMS.Models;
using CommonLayer;
using CommonLayer.Extensions;
using Microsoft.Ajax.Utilities;
using CommonDataLayer.DataAccess;
using PMS.Filters;

namespace PMS.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILogger Logger = CommonLayer.Logger.Register(typeof(HomeController));
        [SkipNoDirectAccess]
        public ActionResult Index()
        {
            Session["EmpList"] = null;
            if (Session["AppUserId"].IsNotNull() && !Session["AppUserId"].ToString().IsNullOrWhiteSpace())
            {

                List<MenuUserRightsModel> records;
                int userId = Convert.ToInt32(Session["AppUserId"]);
                records = GetAllMenuRights(userId, 0);
               
                if (records.IsNotNull() && records.Count > 0)
                {
                    Session["UserMenu"] = records;                
                    records = (List<MenuUserRightsModel>) Session["UserMenu"];
                    
                }
                else
                {
                    Logger.LogInfo("User :" + userId +", do not have access to any menu");
                }
                return View(records.ToList());
            }
            else
            {
                Logger.LogInfo("Missing userID, hence redirecting to Login Page");
                return RedirectToAction("Login", "Account");
            }
        }

        readonly DataSet _ds = new DataSet();
        readonly string _conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public List<MenuUserRightsModel> GetAllMenuRights(int userId, int parentMenuId)
        {
            List<MenuUserRightsModel> menu = null;

            TryCatch.Run(() =>
            {

                using (DBHelper Dbhelper = new DBHelper())
                {

                    DBParameterCollection paramCollection = new DBParameterCollection();
                    paramCollection.Add(new DBParameter("UserId", userId, DbType.Int32));
                    DataTable dtmenuRights = Dbhelper.ExecuteDataTable("sp_GetUserMenuRights", paramCollection, CommandType.StoredProcedure);

                    menu = dtmenuRights.AsEnumerable().Select(row => new MenuUserRightsModel
                    {
                        MenuId = row.Field<int>("MenuId"),
                        UserId = row.Field<int>("UserId"),
                        Access = row.Field<bool>("Access"),                      
                        MenuName = row.Field<string>("MenuName"),
                        PageName = row.Field<string>("PageName"),
                        ParentMenuId = row.Field<int>("ParentMenuId"),                       
                        Icon = row.Field<string>("Icon"),                       
                        RoleId = row.Field<int>("RoleId"),                        
                    }).ToList();
                }
            }).IfNotNull((ex) =>
            {
                Logger.LogError("Error in HomeController  GetAllMenuRights:" + ex.Message + Environment.NewLine + ex.StackTrace);
            });
            return menu;
        }

        [ValidateAntiForgeryToken]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    


    }
}
