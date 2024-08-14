using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PMS.Areas.Masters.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public int EmpId { get; set; }
        [DisplayName("Login Name")]
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmpCode { get; set; }

        public bool Deactive { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Message { get; set; }
    }
}