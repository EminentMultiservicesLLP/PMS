using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PMS.Areas.Masters.Models
{
    public class SubheadsModel
    {
        public int SubHeadId { get; set; }
        public int HeadId { get; set; }       
        public string HeadName { get; set; }
        [DisplayName("SubHead Name")]
        public string SubHeadName { get; set; }
        public int QuestionnaireId { get; set; }
        public string QuestionnaireName { get; set; }
        public bool Deactive { get; set; } 
        public List<ReviewPoint> ReviewPoints { get; set; }
        public double PointGiven { get; set; }
        public string HeadsManView { get; set; }
        public string Message { get; set; }

        public string HeadsRevView { get; set; }
    }


    public class ReviewPoint
    {
        public double Point { get; set; }
        public string Name { get; set; }
    }
}