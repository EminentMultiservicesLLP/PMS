using CommonLayer;
using CommonLayer.Extensions;
using PMS.API.Masters.Interface;
using PMS.API.Masters.Repository;
using PMS.Areas.Masters.Models;
using PMS.Caching;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Areas.Masters.Controllers
{
    public class EmployeeMasterController : Controller
    {
        //
        // GET: /Masters/EmployeeMaster/
        IEmployeeMstInterface _emp;
        private static readonly ILogger _loggger = Logger.Register(typeof(MastersController));
        public EmployeeMasterController(IEmployeeMstInterface emp)
        {
            _emp = emp;
        }
        public EmployeeMasterController()
        {
            _emp = new EmployeeMasterRepository();
        }

        public ActionResult CreateNewEmployeeMasters(EmployeeModel model)
        {

           bool isSuccess = true;
            var isDuplicate = false;
            TryCatch.Run(() =>
            {
                if (model.EmpId == 0)
                {
                    isDuplicate = _emp.CheckDuplicateItem("new", model.EmpId,model.EmpCode,"Employee");
                    if (isDuplicate == false)
                    {
                        var newId = _emp.CreateNewEmployeeMasters(model);
                        model.EmpId = newId;
                        if (model.RoleId !=5)
                        {
                            var newId2 = _emp.CreateNewUserMaster(model, model.EmpId,false);
                            model.UserId = newId2;
                        }
                        isSuccess = true;
                        model.Message = "Employee Data Saved Successfully";
                      //  MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewEmployee.ToString());
                    }
                    else
                   {
                        isSuccess = false;
                       model.Message = "Duplicate Entry or Employee Code Already Exists";

                   }


                }
                else
                {
                      isDuplicate = _emp.CheckDuplicateItem("update",model.EmpId,model.EmpCode,"Employee");
                     if (isDuplicate == false)
                      {
                        model.JoiningDate = DateTime.ParseExact(model.StrJoiningDate, "dd/MM/yy", CultureInfo.InvariantCulture);
                        model.ConfirmationDate = DateTime.ParseExact(model.StrConfirmationDate, "dd/MM/yy", CultureInfo.InvariantCulture);
                        model.LastPromoDate = DateTime.ParseExact(model.StrLastPromoDate, "dd/MM/yy", CultureInfo.InvariantCulture);
                        isSuccess = _emp.UpdateEmployeeMasters(model);

                        // model.EmpId = newId;
                        var newId2=0;
                        if (model.RoleId != 0 )
                        {
                           bool  IsExist = _emp.CheckDuplicateUser(model.EmpId);
                           
                                newId2 = _emp.CreateNewUserMaster(model, model.EmpId, IsExist);
                                //model.UserId = newId2;
                        }
                        else
                        {
                            _emp.DeactivateUser(model.EmpId,model.RoleId);
                        }
                        isSuccess = true;
                       model.Message = "Record updated Successfully";
                      // MemoryCaching.RemoveCacheValue(CachingKeys.GetAllNewEmployee.ToString());
                     }
                    else
                     {
                      isSuccess = false;
                     model.Message = "Duplicate Update Not Allowed";
                     }
                }
            }).IfNotNull(ex =>
            {

                _loggger.LogError("Error in CreateEmployee :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            if (!isSuccess)
                return Json(new
                {
                    success = false,
                    message = model.Message
                });
            else
                return Json(new
                {
                    success = true,
                    message = model.Message
                });


        }
        public ActionResult GetAllRole()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {



                var list = _emp.GetAllRole();
                jResult = Json(list, JsonRequestBehavior.AllowGet);



            }).IfNotNull(ex =>
            {
                _loggger.LogError("Error in GetAllRole :" + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return Json("Error");
            });
            return jResult;

        }
        public ActionResult GetAllEmployee()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {

                var list = GetCachedEmployee();
                jResult = Json(list, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>
            {
                _loggger.LogError("Error in GetAllEmployee :" + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return Json("Error");
            });
            jResult.MaxJsonLength = int.MaxValue;
            return jResult;

        }
        public ActionResult GetAllActiveEmployee()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {
                var list = GetCachedEmployee();
                var jlist = list.Where(m => m.Deactive == false).ToList();
                jResult = Json(jlist, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetAllActiveEmployee :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            jResult.MaxJsonLength = int.MaxValue;
            return jResult;
        }
        public List<EmployeeModel> GetCachedEmployee()
        {
            List<EmployeeModel> emp = null;
            TryCatch.Run(() =>
            {

                //if (!MemoryCaching.CacheKeyExist(CachingKeys.GetAllNewEmployee.ToString()))
                //{
                    var UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                    var list = _emp.GetAllEmployee(UserId);
                    emp = list.ToList();
                    MemoryCaching.AddCacheValue(CachingKeys.GetAllNewEmployee.ToString(), emp);
                //}
                //else
                //{
                    //emp = (List<EmployeeModel>)(MemoryCaching.GetCacheValue(CachingKeys.GetAllNewEmployee.ToString()));
                //}
            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetCachedEmployee :" + ex.Message + Environment.NewLine + ex.StackTrace);
                throw ex;
            });
            return emp;

        }
        public ActionResult Index()
        {
            return View();
        }

    }
}
