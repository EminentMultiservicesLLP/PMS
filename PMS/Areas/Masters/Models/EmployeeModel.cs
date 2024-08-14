using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PMS.Areas.Masters.Models
{
    public class EmployeeModel
    {
        public int EmpId { get; set; }
        public int UserId { get; set; }
        public string EmpName { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        [DisplayName("Joining Date")]
        public DateTime JoiningDate { get; set; }
        [DisplayName("Confirmation Date")]
        public DateTime ConfirmationDate { get; set; }
        [DisplayName("LastPromo Date")]
        public DateTime LastPromoDate { get; set; }
        public string StrJoiningDate { get; set; }
        public string StrConfirmationDate { get; set; }
        public string StrLastPromoDate { get; set; }
        public double Salary { get; set; }
        public int DeptId { get; set; }
        public int AppraiserEmpId { get; set; }
        public int AppraiserTwoEmpId { get; set; }
        public bool State { get; set; }
        public string AppraiserName { get; set; }
        public string AppraiserDesigName { get; set; }
        public string OutletName { get; set; }
        [DisplayName("Employee Code")]
        public string EmpCode { get; set; }
        public bool Deactive { get; set; }
        public int OutletId { get; set; }
        public int RoleId { get; set; }
        public int QuestionnaireId { get; set; }
        public string Questionnairename { get; set; }
        public string RoleName { get; set; }
        public string DeptName { get; set; }
        public string Message { get; internal set; }
        // public string DeptId { get; set; }









    }
}