using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Constants
{
    public static class Constant
    {
        public enum Status
        { 
           InProgress = 1,
           Complete = 2
        }

        /*sql commands*/
        public const string UPDATE = "Update";
        public const string UpdateMulti = "UpdateMulti";
        public const string CREATE = "Create";
        public const string CREATECHILD = "CreateChild";
        public const string SELECT = "Select";
        public const string CREATECHILDINEDITMODE = "CreateChildInEditMode";

        public const string SURVEY_ID = "SurveyId";
        public const string QUESTION_ID = "QuestionId";
        public const string RESPONSE_ID = "ResponseId";
        public const string CURRENT_PAGE = "CurrentPage";
        /*XML tags*/
        public const string SURVEY_RESPONSE = "SurveyResponse";
        public const string RESPONSE_DETAILS = "ResponseDetail";
        /*view names*/
        public const string INDEX_PAGE = "Index";
        public const string SURVEY_INTRODUCTION = "SurveyIntroduction";
        public const string SURVAY_PAGE = "Survey";
        public const string EXCEPTION_PAGE = "Exception";
        

        /*controllers*/
        public const string HOME_CONTROLLER = "Home";
        public const string SURVEY_CONTROLLER = "Survey";

        /*action methods*/
        public const string INDEX = "Index";

        /*Survey page messages*/
        //public const string SURVEY_NOT_EXISTS = "The Survey does not exist. Please check the survey link and try again.";
        //public const string SURVEY_SUBMISSION_MESSAGE = "Thank you! Your survey has been submitted.";
        //public const string SURVEY_SUBMITED_MESSAGE = "This survey has been submitted.";
        //public const string SURVEY_CLOSED_MESSAGE = "This survey is currently closed. Please contact the author of this survey for further assistance.";
       
        public static List<string> MetaDaTaColumnNames()
            { 
          
            List<string> columns = new List<string>();
            columns.Add("_UserEmail");
            columns.Add("_DateUpdated");
            columns.Add("_DateCreated");
           // columns.Add("IsDraftMode");
            columns.Add("_Mode");
            return columns;
            
            }
        public enum UserRole
            {
            User = 1,
            OrgAdmin = 2,
            Admin = 3
            }

    }
}