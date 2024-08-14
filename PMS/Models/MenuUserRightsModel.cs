
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class MenuUserRightsModel
    {
        public MenuUserRightsModel()
        {
            this.parent = new List<ParentMenuRights>();
            this.child = new List<ChildMenuRights>();
        }
        public int MenuId { get; set; }
        public int UserId { get; set; }
        public bool Access { get; set; }
        public bool State { get; set; }       
        public string MenuName { get; set; }
        public string PageName { get; set; }
        public int ParentMenuId { get; set; }      
        public string Icon { get; set; }
        public IEnumerable<ParentMenuRights> parent { get; set; }
        public IEnumerable<ChildMenuRights> child { get; set; }
        public string UpdatedMacName { get; set; }
        public string UpdatedMacID { get; set; }
        public string UpdatedIPAddress { get; set; }
        public Nullable<int> UpdatedByUserID { get; set; }
        public Nullable<System.DateTime> UpdatedON { get; set; }
        public Nullable<int> InsertedBy { get; set; }
        public Nullable<System.DateTime> InsertedON { get; set; }
        public string InsertedMacName { get; set; }
        public string InsertedMacID { get; set; }
        public string InsertedIPAddress { get; set; }       
        public int RoleId { get; set; }       

    }
    public class ParentMenuRights
    {

        public int MenuId { get; set; }
        public long UserId { get; set; }
        public bool Access { get; set; }
        public string MenuName { get; set; }
        public string PageName { get; set; }
        public int ParentMenuId { get; set; }
        public bool State { get; set; }
        public Nullable<int> InsertedBy { get; set; }
        public Nullable<System.DateTime> InsertedON { get; set; }
        public string InsertedMacName { get; set; }
        public string InsertedMacID { get; set; }
        public string InsertedIPAddress { get; set; }
     
    }

    public class ChildMenuRights
    {
        public int MenuId { get; set; }
        public long UserId { get; set; }
        public bool Access { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool DeletePerm { get; set; }
        public bool SuperPerm { get; set; }
        public string MenuName { get; set; }
        public string PageName { get; set; }
        public int ParentMenuId { get; set; }
        public bool State { get; set; }
        public Nullable<int> InsertedBy { get; set; }
        public Nullable<System.DateTime> InsertedON { get; set; }
        public string InsertedMacName { get; set; }
        public string InsertedMacID { get; set; }
        public string InsertedIPAddress { get; set; }
    }
}