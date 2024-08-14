using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.QueryCollection.AdminPanel
{
    public class AdminPanelQueries
    {
        
        public const string GetClientUsersData = "dbsp_ap_GetClientUsersData";
        public const string GetUserCode = "dbsp_ap_GetUserCode";
        public const string GetUserDetails = "dbsp_ap_GetUserDetails";
        public const string SaveUser = "dbsp_ap_SaveUser";


        public const string GetMenuByUser = "dbsp_ap_GetMenuByUser";
        public const string SaveUserAccess = "dbsp_ap_SaveUserAccess";
        public const string GetAllStdSub = "dbsp_ap_GetAllStdSub";

        public const string InsertClient = "dbsp_ap_InsUpdClient";
        public const string GetAllClient = "dbsp_ap_GetAllClient";
        public const string GetAllStates = "dbsp_ap_GetAllStates";
        public const string GetCityById = "dbsp_ap_GetCityById";
        public const string SaveSubStandardAccess = "dbsp_ap_SaveSubStandardAccess";
        public const string GetStdSubjectAccess = "dbsp_ap_GetStdSubjectAccess";
        public const string GetUserDetailsByUserId = "dbsp_ap_GetUserDetailsByUserId";

        public const string SaveBillingConfiguration = "dbsp_SaveBillingConfiguration";
        public const string GetAllBill = "dbsp_GetAllBill";
        public const string GetAllDeductedBills = "dbsp_GetAllDeductedBills";

        public const string SavestudentUser = "dbsp_ap_SaveStudentUser";
        public const string SaveLoginPage = "dbsp_ap_SaveLoginPage";

        public const string GetRoleDetails = "dbsp_ap_GetRoleDetails";
        public const string GetMenuByRole = "dbsp_ap_GetMenuByRole";
        public const string SaveRoleAccess = "dbsp_ap_SaveRoleAccess";
        public const string UpdateMenuRights = "dbsp_ap_UpdateMenuRights";
        public const string SetIsMailNotSent = "dbsp_mst_SetIsMailNotSent";
        public const string GetForgotPassword = "dbsp_mst_GetForgotPassword";
        

        public const string GetChaptSubAccess = "dbsp_ap_GetChaptSubAccess";
        public const string SaveChapterSubAccess = "dbsp_ap_SaveChapterSubAccess";

        public const string GetChapAccessDataForPprCreation = "dbsp_ap_GetChapAccessDataForPprCreation";
        public const string GetUserDetailsForuserAccess = "dbsp_ap_GetUserDetailsForUserAccess";

        //SP For SMSConfiguration
        public const string GetActiveClients = "dbsp_ap_GetActiveClients";
        public const string GetSMSEntities = "dbsp_ap_GetSmsEntities";
        public const string SaveClientEntityLinking = "dbsp_ap_SaveClientEntityLinking";
        public const string GetAllLinkedEntitiesByClientId = "dbsp_ap_GetAllLinkedEntitiesByClientId";
        public const string GetClientEntityList = "dbsp_ap_GetClientEntityList";

        //SP For DashBoard
        public const string GetDashBoardData = "dbsp_ap_GetDashBoardData";
        public const string GetDashBoardReportData = "dbsp_ap_GetDashBoardReportData";

        public const string AllActiveRoles = "dbsp_ap_AllActiveRoles";

        public const string GetAllClientSMSCountData = "dbsp_Ap_GetSMSCount";

        //Sp for CLient Import Data
        public const string ImportClientQuestions = "dbsp_transferCLientQue";
        public const string ImportClientAnswers = "dbsp_transferClientAns";      


    }
}