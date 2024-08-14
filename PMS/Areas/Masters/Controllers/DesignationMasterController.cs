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
    public class DesignationMasterController : Controller
    {


        IDesignationInterface _designation;
        private static readonly ILogger _loggger = Logger.Register(typeof(MastersController));
        public DesignationMasterController(IDesignationInterface designation)
        {
            _designation = designation;
        }
        public DesignationMasterController()
        {
            _designation = new DesignationMasterRepository();
        }

        [HttpPost]
        
        public ActionResult CreateNewDesignationMasters(DesignationModel model)
        {

             bool isSuccess = true;
            var isDuplicate = false;
            TryCatch.Run(() =>
            {
                if (model.DesignationId == 0)
                {
                     isDuplicate = _designation.CheckDuplicateItem(model.DesignationName,"Designation");
                     if (isDuplicate == false)
                      {
                          var newId = _designation.CreateNewDesignationMasters(model);
                          model.DesignationId = newId;
                          isSuccess = true;
                          model.Message = "Designation Data Saved Successfully";
                          MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewDesignation.ToString());
                      }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Designation Name Already Exists";

                    }

                }
                else
                {
                    isDuplicate = _designation.CheckDuplicateUpdate(model, "Designation");
                    if (isDuplicate == false)
                    {
                        isSuccess = _designation.UpdateDesignationMasters(model);
                        isSuccess = true;
                        model.Message = "Record updated Successfully";
                        MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewDesignation.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Designation Name Already Exists";
                    }
                }

            }).IfNotNull(ex =>
            {

                _loggger.LogError("Error in Create Designation :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            if (!isSuccess)
                return Json(new { success = false, message = model.Message });
            else
                return Json(new { success = true, message = model.Message });

           
        }

        public ActionResult GetAllDesignation()
        {
            JsonResult jResult = null; 
            TryCatch.Run(() =>
            {

               var list = GetCachedDesignation();
               jResult = Json(list, JsonRequestBehavior.AllowGet);
              
            }).IfNotNull(ex =>
            {
                _loggger.LogError("Error in GetAllDesignation :" + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return Json("Error");
            });
            return jResult;

        }
        public ActionResult GetAllActiveDesignation()
        {
            JsonResult jResult=null;
            TryCatch.Run(() =>
            {
                var list = GetCachedDesignation();
                var jlist = list.Where(m => m.Deactive == false).ToList();
                jResult = Json(jlist, JsonRequestBehavior.AllowGet);
                
            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetAllActiveDesignation :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            return jResult;
        }
        public List<DesignationModel> GetCachedDesignation()
        {
            List<DesignationModel> designation=null;
            TryCatch.Run(() =>
            {

                 if (!MemoryCaching.CacheKeyExist(CachingKeys.GetAllNewDesignation.ToString()))
                 {
                    var UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                    var list = _designation.GetAllDesignation(UserId);
                    designation = list.ToList();
                  MemoryCaching.AddCacheValue(CachingKeys.GetAllNewDesignation.ToString(), designation);
                 }
                else
                 {
                    designation = (List<DesignationModel>)(MemoryCaching.GetCacheValue(CachingKeys.GetAllNewDesignation.ToString()));
                 }
            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetCachedDesignation :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            });
            return designation;

        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
