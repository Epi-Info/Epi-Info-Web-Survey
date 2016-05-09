using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Web.Mvc;
namespace Epi.Web.MVC.Models
{
    public class PublishModel
    {
        private string _Path;
        private string _OrganizationKey;
        private string _SurveyKey;
        private bool _UpdateExisting;
        private string _EndDate;
        private string _SurveyName;
        private bool  _SuccessfulPublish;
        private string _FileName;
        private string _UserPublishKey;
        private List<string> _SurveyNameList;
         
        public PublishModel()
        {
          
        }

        [Required(ErrorMessage = "File path is required.")]
        public string Path
            {
                get { return _Path; }
                set { _Path = value; }
            }
         [Required(ErrorMessage = "Organization key is required.")]
         [RegularExpression(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", ErrorMessage = "Invalid Organization key.")]
         public string OrganizationKey
         {
             get { return _OrganizationKey; }
             set { _OrganizationKey = value; }
         }
         [Required(ErrorMessage = "Security Token is required.")]
         [RegularExpression(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", ErrorMessage = "Invalid Security Token.")]
        
         public string UserPublishKey
         {
             get { return _UserPublishKey; }
             set { _UserPublishKey = value; } 
         }

         // [RegularExpression(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", ErrorMessage = "Invalid Organization key.")]
         
         //[Required(ErrorMessage = "Survey Id is required.")]
         
        public string SurveyKey
         {
             get { return _SurveyKey; }
             set { _SurveyKey = value; }
         }

         public bool UpdateExisting
         {
             get { return _UpdateExisting; }
             set { _UpdateExisting = value; }
         }
       [Required(ErrorMessage = "End Date is required.")]
         public string EndDate
         {
             get { return _EndDate; }
             set { _EndDate = value; }
         }
         [Required(ErrorMessage = "Survey Name is required.")]
         public string SurveyName
         {
             get { return _SurveyName; }
             set { _SurveyName = value; }
         }
         [Required(ErrorMessage = "File Name is required.")]
         public string FileName
         {
             get { return _FileName; }
             set { _FileName = value; }
         }
         public bool SuccessfulPublish
         {
             get { return _SuccessfulPublish; }
             set { _SuccessfulPublish = value; }
         }

         public string SelectedValue { get; set; }
         public IEnumerable<SelectListItem> SurveyNameList
         {
             get;
             set;

         }

         
         public string SurveyURL { get; set; }
         public string TimeElapsed { get; set; }
         public int RecordCount { get; set; }
         public string SurveyMode { get; set; }
         public bool IsValidOrg { get; set; }
          
    }
}