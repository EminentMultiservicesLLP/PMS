using CommonLayer;
using CommonLayer.Extensions;
using PMS.API.Masters.Interface;
using PMS.API.Masters.Repository;
using PMS.Areas.Masters.Models;
using PMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Areas.Masters.Controllers
{
    [CheckSession]
    [ValidateHeaderAntiForgeryTokenAttribute]
    public class CommonMasterController : Controller
    {
        //
        // GET: /Masters/CommonMaster/
 
        ICommonMaster _commonMaster;
        private static readonly ILogger Loggger = Logger.Register(typeof(CommonMasterController));

        public CommonMasterController()
        {
            _commonMaster = new CommonMasterRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllDesignation()
        {
            JsonResult jsonResult = null;
            List<CommonMasterModel> list = new List<CommonMasterModel>();
            TryCatch.Run(() =>
            {
                list = _commonMaster.GetAllDesignation();
                jsonResult = Json(new { data = list }, JsonRequestBehavior.AllowGet);
            }).IfNotNull(ex =>
            {
                Loggger.LogError("Error in class:CommonMasterController method :GetRoomEntitlementList :" + Environment.NewLine + ex.StackTrace);
            });

            return jsonResult;

        }

        public ActionResult GetAllGrade()
        {
            JsonResult jsonResult = null;
            List<CommonMasterModel> list = new List<CommonMasterModel>();
            TryCatch.Run(() =>
            {
                list = _commonMaster.GetAllGrade();
                jsonResult = Json(new { data = list }, JsonRequestBehavior.AllowGet);
            }).IfNotNull(ex =>
            {
                Loggger.LogError("Error in class:CommonMasterController method :GetRoomEntitlementList :" + Environment.NewLine + ex.StackTrace);
            });

            return jsonResult;
        }
        public ActionResult GetAllOutlet()
        {
            JsonResult jsonResult = null;
            List<CommonMasterModel> list = new List<CommonMasterModel>();
            TryCatch.Run(() =>
            {
                list = _commonMaster.GetAllOutlet();
                jsonResult = Json(new { data = list }, JsonRequestBehavior.AllowGet);
            }).IfNotNull(ex =>
            {
                Loggger.LogError("Error in class:CommonMasterController method :GetAllOutlet :" + Environment.NewLine + ex.StackTrace);
            });

            return jsonResult;

        }

        

    }
}
