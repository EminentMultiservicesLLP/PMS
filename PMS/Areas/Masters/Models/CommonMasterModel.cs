using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Areas.Masters.Models
{
    public class CommonMasterModel
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public int GradePoints { get; set; }
        public bool Deactive { get; set; }
        public int OutletId { get; set; }
        public string OutletName { get; set; }
    }
}