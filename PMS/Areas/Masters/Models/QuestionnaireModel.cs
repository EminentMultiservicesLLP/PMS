using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace PMS.Areas.Masters.Models
{
    public class QuestionnaireModel
    {
        public int QuestionnaireId { get; set; }
        [DisplayName ("Questionnaire Name")]
        public string QuestionnaireName { get; set; }
        public bool Deactive { get; set; }

        public string Message { get; set; }
    }
}