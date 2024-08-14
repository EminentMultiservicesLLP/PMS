using CommonLayer;
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
    public class MastersController : Controller
    {
        //
        // GET: /Masters/Masters/



        IDesignationInterface _designation;
        private static readonly ILogger _loggger = Logger.Register(typeof(MastersController));
        public MastersController(IDesignationInterface designation)
        {
         _designation = designation;
        }
        public MastersController()
        {
         _designation = new DesignationMasterRepository();
        }

        
        public ActionResult Index()
        {
            return View();
        }
        

        public ActionResult DesignationMaster()
        {
            return PartialView();
        }
        public ActionResult DepartmentMaster()
        {
            return PartialView();
        }
        public ActionResult GradeMaster()
        {
            return PartialView();
        }
        public ActionResult OutletMaster()
        {
            return PartialView();
        }
        public ActionResult HeadsMaster()
        {
            return PartialView();
        }
        public ActionResult SubHeadsMaster()
        {
            return PartialView();
        }
        public ActionResult EmployeeMaster()
        {
            return PartialView();
        }

        public ActionResult QuestionnaireMaster()
        {
            return PartialView();
        }

        public ActionResult UsersMaster()
        {
            return PartialView();
        }
        
    }
}
