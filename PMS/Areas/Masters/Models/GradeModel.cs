using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PMS.Areas.Masters.Models
{
    public class GradeModel
    {
        public int GradeId { get; set; }
        [DisplayName("Grade Name")]
        public string GradeName { get; set; }
        [DisplayName("Grade Points")]
        public int GradePoints { get; set; }
        public bool Deactive { get; set; }
        public string Message { get; set; }


    }
}