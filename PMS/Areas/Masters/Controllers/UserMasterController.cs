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
    public class UserMasterController : Controller
    {
        //
        // GET: /Masters/UserMaster/
        IUserInterface _user;
        private static readonly ILogger _loggger = Logger.Register(typeof(MastersController));
        public UserMasterController(IUserInterface user)
        {
            _user = user;
        }
        public UserMasterController()
        {
            _user = new UserMasterRepository();
        }
        [HttpPost]
        public ActionResult UpdateUserMasters(UserModel model)
        {

            bool isSuccess = true;
            var isDuplicate = false;
            TryCatch.Run(() =>
            {
                
                    isDuplicate = _user.CheckDuplicateUpdate(model.UserId,model.EmpId, "User");
                    if (isDuplicate == false)
                    {
                        isSuccess = _user.UpdateUserMasters(model);
                        isSuccess = true;
                        model.Message = "Record updated Successfully";
                        MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewUser.ToString());
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "No Update Found";
                    }
                


            }).IfNotNull(ex =>
            {

                _loggger.LogError("Error in Create User :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            if (!isSuccess)
                return Json(new { success = false, message = model.Message });
            else
                return Json(new { success = true, message = model.Message });


        }


    
    public ActionResult GetAllUser()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {

                var list = GetCachedUser();
                jResult = Json(list, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>
            {
                _loggger.LogError("Error in GetAllUser :" + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return Json("Error");
            });
            return jResult;

        }
        public ActionResult GetAllActiveUser()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {
                var list = GetCachedUser();
                var jlist = list.Where(m => m.Deactive == false).ToList();
                jResult = Json(jlist, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetAllActiveUser :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            return jResult;
        }
        public List<UserModel> GetCachedUser()
        {
            List<UserModel> user = null;
            TryCatch.Run(() =>
            {

              //  if (!MemoryCaching.CacheKeyExist(CachingKeys.GetAllNewUser.ToString()))
                {
                    // var UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                    var list = _user.GetAllUser();
                    user = list.ToList();
                 //   MemoryCaching.AddCacheValue(CachingKeys.GetAllNewUser.ToString(), user);
                }
               // else
                {
                  //  user = (List<UserModel>)(MemoryCaching.GetCacheValue(CachingKeys.GetAllNewUser.ToString()));
                }
            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetCachedUser :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            });
            return user;

        }


        public ActionResult Index()
        {
            return View();
        }

    }
}
