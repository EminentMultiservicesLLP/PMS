using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PMS.Areas.Masters.Models
{
    public class DesignationModel
    {
        public int DesignationId { get; set; }
        [DisplayName("Designation Name")]
        public string DesignationName { get; set; }
        public bool Deactive { get; set; }

        public string Message { get; set; }
    }
}