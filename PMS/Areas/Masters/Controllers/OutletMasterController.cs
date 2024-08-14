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
    public class OutletMasterController : Controller
    {
        //
        // GET: /Masters/OutletMaster/
        IOutletInterface _outlet;
        private static readonly ILogger _loggger = Logger.Register(typeof(MastersController));
        public OutletMasterController(IOutletInterface outlet)
        {
            _outlet = outlet;
        }
        public OutletMasterController()
        {
            _outlet = new OutletMasterRepository();
        }

        [HttpPost]

        public ActionResult CreateNewOutletMasters(OutletModel model)
        {

            bool isSuccess = true;
            var isDuplicate = false;
            TryCatch.Run(() =>
            {
                if (model.OutletId == 0)
                {
                    isDuplicate = _outlet.CheckDuplicateItem(model.OutletName, "Outlet");
                    if (isDuplicate == false)
                    {
                        var newId = _outlet.CreateNewOutletMasters(model);
                        model.OutletId = newId;
                        isSuccess = true;
                        model.Message = "Outlet Data Saved Successfully";
                        MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewOutlet.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Outlet Name Already Exists";

                    }

                }
                else
                {
                    isDuplicate = _outlet.CheckDuplicateUpdate(model, "Outlet");
                    if (isDuplicate == false)
                    {
                        isSuccess = _outlet.UpdateOutletMasters(model);
                        isSuccess = true;
                        model.Message = "Record updated Successfully";
                        MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewOutlet.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Outlet Name Already Exists";
                    }
                }

            }).IfNotNull(ex =>
            {

                _loggger.LogError("Error in Create Outlet :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            if (!isSuccess)
                return Json(new { success = false, message = model.Message });
            else
                return Json(new { success = true, message = model.Message });


        }

        public ActionResult GetAllOutlet()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {

                var list = GetCachedOutlet();
                jResult = Json(list, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>
            {
                _loggger.LogError("Error in GetAllOutlet :" + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return Json("Error");
            });
            return jResult;

        }
        public ActionResult GetAllActiveOutlet()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {
                var list = GetCachedOutlet();
                var jlist = list.Where(m => m.Deactive == false).ToList();
                jResult = Json(jlist, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetAllActiveOutlet :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            return jResult;
        }
        public List<OutletModel> GetCachedOutlet()
        {
            List<OutletModel> outlet = null;
            TryCatch.Run(() =>
            {

                if (!MemoryCaching.CacheKeyExist(CachingKeys.GetAllNewOutlet.ToString()))
                {
                    var UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                    var list = _outlet.GetAllOutlet(UserId);
                    outlet = list.ToList();
                    MemoryCaching.AddCacheValue(CachingKeys.GetAllNewOutlet.ToString(), outlet);
                }
                else
                {
                    outlet = (List<OutletModel>)(MemoryCaching.GetCacheValue(CachingKeys.GetAllNewOutlet.ToString()));
                }
            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetCachedOutlet :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            });
            return outlet;

        }

        


        public ActionResult Index()
        {
            return View();
        }

    }
}
