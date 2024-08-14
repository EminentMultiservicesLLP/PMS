using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMS.Areas.Masters.Models
{
  
        public class DepartmentModel
        {
           public int DeptId { get; set; }
           [DisplayName("Department Name")]
           public string DeptName { get; set; }
           public bool Deactive { get; set; }
          
        public string Message { get; set; }
        }
    
}
