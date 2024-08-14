using CommonLayer;
using CommonLayer.Extensions;
using PMS.API.Masters.Interface;
using PMS.API.Masters.Repository;
using PMS.Areas.Masters.Models;
using PMS.Caching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Areas.Masters.Controllers
{
    public class SubHeadsMasterController : Controller
    {
        //
        // GET: /Masters/SubHeadsMaster/
        ISubHeadsInterface _sheads;
        private static readonly ILogger _loggger = Logger.Register(typeof(MastersController));
        public SubHeadsMasterController(ISubHeadsInterface sheads)
        {
            _sheads = sheads;
        }
        public SubHeadsMasterController()
        {
            _sheads = new SubHeadsMasterRepository();
        }
        public ActionResult CreateNewSubHeadsMasters(SubheadsModel model)
        {

            bool isSuccess = true;
             var isDuplicate = false;
            TryCatch.Run(() =>
            {
                if (model.SubHeadId == 0)
                {
                    isDuplicate = _sheads.CheckDuplicateItem(model.SubHeadName, model.HeadId, "SubHeads");
                     if (isDuplicate == false)
                    {
                        var newId = _sheads.CreateNewSubHeadsMasters(model);
                        model.HeadId = newId;
                        isSuccess = true;
                        model.Message = "Head Name Saved Successfully";
                        MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewSubHeads.ToString());
                    }
                     else
                    {
                         isSuccess = false;
                        model.Message = "Head Name Or Sub Head Name Already Exists";

                    }

                    
                }
                else
                {
                    isDuplicate = _sheads.CheckDuplicateUpdate(model.SubHeadName, model.HeadId, "SubHeads");
                    if (isDuplicate == false)
                    {
                        isSuccess = _sheads.UpdateSubHeadsMasters(model);
                        isSuccess = true;
                        model.Message = "Record updated Successfully";
                        MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewSubHeads.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Heads Name And SubHead Name Already Exists";
                   }
                }
            }).IfNotNull(ex =>
           {

               _loggger.LogError("Error in Create SubHeads :" + ex.Message + Environment.NewLine + ex.StackTrace);
               return Json("Error");
           });
            if (!isSuccess)
                return Json(new { success = false, message = model.Message});
            else
                return Json(new { success = true, message = model.Message});

        
        }

        public ActionResult GetAllHeadName(int questionnaireId)
        {
           JsonResult jResult = null;
            TryCatch.Run(() =>
            {

                
             
                var list = _sheads.GetAllHeadName(questionnaireId);
                jResult = Json(list, JsonRequestBehavior.AllowGet);



            }).IfNotNull(ex =>
            {
                _loggger.LogError("Error in GetAllSubHeadName :" + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return Json("Error");
            });
            return jResult ;

        }
       
        

        public ActionResult GetAllSubHeads()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {

                var list = GetCachedSubHeads();
                jResult = Json(list, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>
            {
                _loggger.LogError("Error in GetAllSubHeads :" + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return Json("Error");
            });
            return jResult;

        }
        public ActionResult GetAllActiveSubHeads()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {
                var list = GetCachedSubHeads();
                var jlist = list.Where(m => m.Deactive == false).ToList();
                jResult = Json(jlist, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetAllActiveSubHeads :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            return jResult;
        }
        public List<SubheadsModel> GetCachedSubHeads()
        {
            List<SubheadsModel> sheads = null;
            TryCatch.Run(() =>
            {

               // if (!MemoryCaching.CacheKeyExist(CachingKeys.GetAllNewSubHeads.ToString()))
                {
                    var UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                    var list = _sheads.GetAllSubHeads(UserId);
                    sheads = list.ToList();
                   // MemoryCaching.AddCacheValue(CachingKeys.GetAllNewSubHeads.ToString(), sheads);
                }
               // else
                {
                //    sheads = (List<SubheadsModel>)(MemoryCaching.GetCacheValue(CachingKeys.GetAllNewSubHeads.ToString()));
                }
            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetCachedHeads :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            });
            return sheads;

        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
