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
    public class HeadsMasterController : Controller
    {
        //
        // GET: /Masters/HeadsMaster/

        IHeadsInterface _heads;
        private static readonly ILogger _loggger = Logger.Register(typeof(MastersController));
        public HeadsMasterController(IHeadsInterface heads)
        {
            _heads = heads;
        }
        public HeadsMasterController()
        {
            _heads = new HeadsMasterRepository();
        }
        [HttpPost]

        public ActionResult CreateNewHeadsMasters(HeadsModel model)
        {

            bool isSuccess = true;
            var isDuplicate = false;
            TryCatch.Run(() =>
            {
                if (model.HeadId == 0)
                {
                    isDuplicate = _heads.CheckDuplicateItem(model.HeadName,model.QuestionnaireId, "Heads");
                    if (isDuplicate == false)
                    {
                        var newId = _heads.CreateNewHeadsMasters(model);
                        model.HeadId = newId;
                        isSuccess = true;
                        model.Message = "Head Name Saved Successfully";
                        MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewHeads.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Head Name Already Exists";

                    }

                }
                else
                {
                    isDuplicate = _heads.CheckDuplicateUpdate(model.HeadName,model.QuestionnaireId, "Heads");
                    if (isDuplicate == false)
                    {
                        isSuccess = _heads.UpdateHeadsMasters(model);
                        isSuccess = true;
                        model.Message = "Record updated Successfully";
                        MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewHeads.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Heads Name Already Exists";
                    }
                }


            }).IfNotNull(ex =>
            {

                _loggger.LogError("Error in Create Heads :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            if (!isSuccess)
                return Json(new { success = false, message = model.Message });
            else
                return Json(new { success = true, message = model.Message });


        }

        public ActionResult GetAllHeads()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {

                var list = GetCachedHeads();
                jResult = Json(list, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>
            {
                _loggger.LogError("Error in GetAllHeads :" + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return Json("Error");
            });
            return jResult;

        }
        public ActionResult GetAllActiveHeads()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {
                var list = GetCachedHeads();
                var jlist = list.Where(m => m.Deactive == false).ToList();
                jResult = Json(jlist, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetAllActiveHeads :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            return jResult;
        }
        public List<HeadsModel> GetCachedHeads()
        {
            List<HeadsModel> heads = null;
            TryCatch.Run(() =>
            {

                //if (!MemoryCaching.CacheKeyExist(CachingKeys.GetAllNewHeads.ToString()))
                {
                   // var UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                    var list = _heads.GetAllHeads();
                    heads = list.ToList();
                  //  MemoryCaching.AddCacheValue(CachingKeys.GetAllNewHeads.ToString(), heads);
                }
               // else
                {
              //      heads = (List<HeadsModel>)(MemoryCaching.GetCacheValue(CachingKeys.GetAllNewHeads.ToString()));
                }
            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetCachedHeads :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            });
            return heads;

        }


        public ActionResult Index()
        {
            return View();
        }

    }
}
