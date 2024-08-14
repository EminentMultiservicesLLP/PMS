using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.API.Masters.Interface;
using CommonLayer;
using PMS.Areas.Masters.Models;
using PMS.API.Masters.Repository;
using PMS.Caching;
using CommonLayer.Extensions;

namespace PMS.Areas.Masters.Controllers
{
    public class QuestionnaireController : Controller
    {
        //
        // GET: /Masters/Questionnaire/
                
        IQuestionnaireInterface _Questn;
        private static readonly ILogger _loggger = Logger.Register(typeof(MastersController));
        public QuestionnaireController(IQuestionnaireInterface Questn)
        {
            _Questn = Questn;
        }
        public QuestionnaireController()
        {
            _Questn = new QuestionnaireRepository();
        }

        [HttpPost]
        public ActionResult CreateNewQUestnMasters(QuestionnaireModel model)
        {

            bool isSuccess = true;
            var isDuplicate = false;
            TryCatch.Run(() =>
            {
                if (model.QuestionnaireId == 0)
                {
                    isDuplicate = _Questn.CheckDuplicateItem(model.QuestionnaireName, "Questionnaire");
                    if (isDuplicate == false)
                    {
                        var newId = _Questn.CreateNewQuestnMasters(model);
                        model.QuestionnaireId = newId;
                        isSuccess = true;
                        model.Message = "Questionnaire Data Saved Successfully";
                      
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Questionnaire Name Already Exists";

                    }

                }
                else
                {
                    isDuplicate = _Questn.CheckDuplicateUpdate(model, "Questionnaire");
                    if (isDuplicate == false)
                    {
                        isSuccess = _Questn.UpdateQuestnMasters(model);
                        isSuccess = true;
                        model.Message = "Record updated Successfully";
                       
                    }
                    else
                    {
                        isSuccess = false;
                        model.Message = "Questionnaire Name Already Exists";
                    }
                }

            }).IfNotNull(ex =>
            {

                _loggger.LogError("Error in Create Questionnaire :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            if (!isSuccess)
                return Json(new { success = false, message = model.Message });
            else
                return Json(new { success = true, message = model.Message });


        }

        public ActionResult GetAllQuestionnaire()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {
                
                var list = _Questn.GetAllQuestionnaire().ToList();                
                jResult = Json(list, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>
            {
                _loggger.LogError("Error in GetAllQuestionnaire :" + ex.Message + Environment.NewLine + ex.StackTrace.ToString());
                return Json("Error");
            });
            return jResult;

        }
        public ActionResult GetAllActiveQuestion()
        {
            JsonResult jResult = null;
            TryCatch.Run(() =>
            {;
                var list = _Questn.GetAllQuestionnaire();                
                var jlist = list.Where(m => m.Deactive == false).ToList();
                jResult = Json(jlist, JsonRequestBehavior.AllowGet);

            }).IfNotNull(ex =>

            {
                _loggger.LogError("Error in GetAllActiveQuestion :" + ex.Message + Environment.NewLine + ex.StackTrace);
                return Json("Error");
            });
            return jResult;
        }
      

    }
}
