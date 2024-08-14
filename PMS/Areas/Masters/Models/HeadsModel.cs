using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PMS.Areas.Masters.Models
{
    public class HeadsModel
    {
        public int HeadId { get; set; }
        public int QuestionnaireId { get; set; }
        [DisplayName("Head Name")]
        public string HeadName { get; set; }
        public bool Deactive { get; set; }

        [DisplayName("Questionnaire Name")]
        public string QuestionnaireName { get; set; }
        public string Message { get; set; }


    }
}