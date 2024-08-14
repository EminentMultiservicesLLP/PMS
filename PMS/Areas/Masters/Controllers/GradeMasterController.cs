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
    public class GradeMasterController : Controller
    {
        //
        // GET: /Masters/GradeMaster/
        IGradeInterface _grade;
        private static readonly ILogger _loggger = Logger.Register(typeof(MastersController));
        public GradeMasterController(IGradeInterface grade)
        {
            _grade = grade;
        }
        public GradeMasterController()
        {
            _grade = new GradeMasterRepository();
        }

        [HttpPost]

        public ActionResult CreateNewGradeMasters(GradeModel model)
        {

            bool isSuccess = true;
            var isDuplicate = false;
            TryCatch.Run(() =>
            {
                if (model.GradeId == 0)
                {
                    isDuplicate = _grade.CheckDuplicateItem(model.GradeName, "Grade");
                    if (isDuplicate == false)
                    {
                        var newId = _grade.CreateNewGradeMasters(model);
                        model.GradeId = newId;
                        isSuccess = true;
                        model.Message = "Grade Name Saved Successfully";
                        MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewGrade.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Grade Name Already Exists";

                    }

                }
                else
                {
                    isDuplicate = _grade.CheckDuplicateUpdate(model, "Grade");
                    if (isDuplicate == false)
                    {
                        isSuccess = _grade.UpdateGradeMasters(model);
                        isSuccess = true;
                        model.Message = "Record updated Successfully";
                        MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewGrade.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Grade Name Already Exists";
                    }
                }

            }).IfNotNull(ex =>
            {

                _loggger.LogError("Error in Create Grade :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            if (!isSuccess)
                return Json(new { success = false, message = model.Message });
            else
                return Json(new { success = true, message = model.Message });


        }
        public ActionResult GetAllGrade()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {

                var list = GetCachedGrade();
                jResult = Json(list, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>
            {
                _loggger.LogError("Error in GetAllGrade :" + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return Json("Error");
            });
            return jResult;

        }
        public ActionResult GetAllActiveGrade()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {
                var list = GetCachedGrade();
                var jlist = list.Where(m => m.Deactive == false).ToList();
                jResult = Json(jlist, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetAllGrade :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            return jResult;
        }
        public List<GradeModel> GetCachedGrade()
        {
            List<GradeModel> grade = null;
            TryCatch.Run(() =>
            {

                if (!MemoryCaching.CacheKeyExist(CachingKeys.GetAllNewGrade.ToString()))
                {
                    var UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                    var list = _grade.GetAllGrade(UserId);
                    grade = list.ToList();
                    MemoryCaching.AddCacheValue(CachingKeys.GetAllNewGrade.ToString(), grade);

                }
                else
                {
                    grade = (List<GradeModel>)(MemoryCaching.GetCacheValue(CachingKeys.GetAllNewGrade.ToString()));
                }
            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetCachedGrade :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            });
            return grade;

        }












        public ActionResult Index()
        {
            return View();
        }

    }
}
