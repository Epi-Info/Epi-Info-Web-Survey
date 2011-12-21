using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epi.Web.Models;
using Epi.Web.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.Common.Criteria;
using Epi.Web.Models.Constants;
using MvcDynamicForms.Demo.Models;
using Epi.Web.Models.ModelObject;
using Epi.Web.Models.Utility;

namespace Epi.Web.Controllers
{
    public class FinalController : Controller
    {
        

        //declare SurveyTransactionObject object
        private SurveyTransactionObject _surveyTransactionObj;
        /// <summary>
        /// Injectinting SurveyTransactionObject through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        public FinalController(SurveyTransactionObject surveyTransactionObj)
        {
           
            _surveyTransactionObj = surveyTransactionObj;
        }
        public ActionResult Index(string surveyId)
        {

            SurveyInfoModel surveyInfoModel = _surveyTransactionObj.GetSurveyInfoModel(surveyId);
            return View(Epi.Web.Models.Constants.Constant.SUBMIT_PAGE, surveyInfoModel);
            
            
        }

        

    }
}
