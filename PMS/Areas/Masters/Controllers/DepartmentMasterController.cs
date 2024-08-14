using CommonLayer;
using CommonLayer.Extensions;
using PMS.API.Masters.Interface;
using PMS.API.Masters.Repository;
using PMS.Areas.Masters.Models;
using PMS.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Areas.Masters.Controllers
{
    public class DepartmentMasterController : Controller
    {
        //
        // GET: /Masters/DepartmentMaster/
        IDepartmentInterface _department;
        private static readonly ILogger _loggger = Logger.Register(typeof(MastersController));
        public DepartmentMasterController(IDepartmentInterface department)
        {
            _department = department;
        }
        public DepartmentMasterController()
        {
            _department = new DepartmentMasterRepository();
        }

        [HttpPost]

        public ActionResult CreateNewDepartmentMasters(DepartmentModel model)
        {

            bool isSuccess = true;
            var isDuplicate = false;
            TryCatch.Run(() =>
            {
                if (model.DeptId == 0)
                {
                    isDuplicate = _department.CheckDuplicateItem(model.DeptName, "Department");
                    if (isDuplicate == false)
                    {
                        var newId = _department.CreateNewDepartmentMasters(model);
                        model.DeptId = newId;
                        isSuccess = true;
                        model.Message = "Department Name Saved Successfully";
                       MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewDepartment.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Department Name Already Exists";

                    }

                }
                else
                {
                    isDuplicate = _department.CheckDuplicateUpdate(model, "Department");
                    if (isDuplicate == false)
                    {
                        isSuccess = _department.UpdateDepartmentMasters(model);
                        isSuccess = true;
                        model.Message = "Record updated Successfully";
                        MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewDepartment.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Department Name Already Exists";
                    }
                }

            }).IfNotNull(ex =>
            {

                _loggger.LogError("Error in Create Department :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            if (!isSuccess)
                return Json(new { success = false, message = model.Message });
            else
                return Json(new { success = true, message = model.Message });


        }
        public ActionResult GetAllDepartment()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {

                var list = GetCachedDepartment();
                jResult = Json(list, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>
            {
                _loggger.LogError("Error in GetAllDepartment :" + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return Json("Error");
            });
            return jResult;

        }
        public ActionResult GetAllActiveDepartment()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {
                var list = GetCachedDepartment();
                var jlist = list.Where(m => m.Deactive == false).ToList();
                jResult = Json(jlist, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetAllDepartment :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            return jResult;
        }
        public List<DepartmentModel> GetCachedDepartment()
        {
            List<DepartmentModel> department = null;
            TryCatch.Run(() =>
            {

                 if (!MemoryCaching.CacheKeyExist(CachingKeys.GetAllNewDepartment.ToString()))
                 {
                      var UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                      var list = _department.GetAllDepartment(UserId);
                      department = list.ToList();
                      MemoryCaching.AddCacheValue(CachingKeys.GetAllNewDepartment.ToString(), department);

                  }
                else
                {
                   department = (List<DepartmentModel>)(MemoryCaching.GetCacheValue(CachingKeys.GetAllNewDepartment.ToString()));
                 }
            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetCachedDepartment :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            });
            return department;

        }


        public ActionResult Index()
        {
            return View();
        }

    }
}
