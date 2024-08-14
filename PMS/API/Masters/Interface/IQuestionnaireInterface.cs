using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Areas.Masters.Models;

namespace PMS.API.Masters.Interface
{
    public interface IQuestionnaireInterface
    {
        int CreateNewQuestnMasters(QuestionnaireModel model);
        IEnumerable<QuestionnaireModel> GetAllQuestionnaire();
        IEnumerable<QuestionnaireModel> GetAllActiveQuestion();
        bool CheckDuplicateItem(string OutletName, string type);
        bool CheckDuplicateUpdate(QuestionnaireModel model, string type);
        bool UpdateQuestnMasters(QuestionnaireModel model);
    }
}