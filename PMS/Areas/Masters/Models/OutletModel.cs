using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PMS.Areas.Masters.Models
{
    public class OutletModel
    {
        public int OutletId { get; set; }
        [DisplayName("Outlet Name")]
        public string OutletName { get; set; }
        public bool Deactive { get; set; }

        public string Message { get; set; }

    }
}