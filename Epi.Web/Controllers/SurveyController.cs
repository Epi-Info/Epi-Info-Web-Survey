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
    public class SurveyController : Controller
    {


        

        //declare SurveyTransactionObject object
        private SurveyTransactionObject _surveyTransactionObj;
        /// <summary>
        /// Injectinting SurveyTransactionObject through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        
        public SurveyController(SurveyTransactionObject surveyTransactionObj)
        {
            _surveyTransactionObj = surveyTransactionObj;
        }
        
        
                
       
       /// <summary>
       /// create the new resposeid and put it in temp data. create the form object. create the first survey response
       /// </summary>
       /// <param name="surveyId"></param>
       /// <returns></returns>
 
        
        public ActionResult Index(string surveyId)
        {
           

            //create the responseid
            Guid ResponseID = Guid.NewGuid();
            
            //get the survey form
            var form = _surveyTransactionObj.GetSurveyFormData(surveyId, this.GetCurrentPage(), this.GetCurrentSurveyAnswer());

            //put the ResponseId in Temp data for later use
            TempData[Epi.Web.Models.Constants.Constant.RESPONSE_ID] = ResponseID.ToString();

            // create the first survey response
            _surveyTransactionObj.CreateSurveyAnswer(surveyId, ResponseID.ToString());
            return View(Epi.Web.Models.Constants.Constant.INDEX_PAGE, form);          
        }
        [HttpPost]
        public ActionResult Index(SurveyInfoModel surveyInfoModel,string Submit)
        {
            
            //get the survey form
            MvcDynamicForms.Form form = _surveyTransactionObj.GetSurveyFormData(surveyInfoModel.SurveyId, this.GetCurrentPage(), this.GetCurrentSurveyAnswer());
            //Update the model
            UpdateModel(form);
            if (form.Validate())
            {
                _surveyTransactionObj.UpdateSurveyResponse(surveyInfoModel, TempData[Epi.Web.Models.Constants.Constant.RESPONSE_ID].ToString(), form);
                
                return RedirectToAction("Index", "Final");
            }
            else
            {
                return View(Epi.Web.Models.Constants.Constant.INDEX_PAGE, form);
            }

            
        }
      


        



        
        private int GetCurrentPage()
        {
            int CurrentPage = 1;
            if (TempData.ContainsKey(Epi.Web.Models.Constants.Constant.CURRENT_PAGE) && TempData[Epi.Web.Models.Constants.Constant.CURRENT_PAGE] != null)
            {
                int.TryParse(TempData[Epi.Web.Models.Constants.Constant.CURRENT_PAGE].ToString(), out CurrentPage);
            }

            return CurrentPage;
        }

        private Epi.Web.Common.DTO.SurveyAnswerDTO GetCurrentSurveyAnswer()
        {
            Epi.Web.Common.DTO.SurveyAnswerDTO result = null;

            //if (TempData.ContainsKey(Epi.Web.Models.Constants.Constant.RESPONSE_ID) && TempData[Epi.Web.Models.Constants.Constant.RESPONSE_ID] != null
            //                                                                   && !string.IsNullOrEmpty(TempData[Epi.Web.Models.Constants.Constant.RESPONSE_ID].ToString()))
            //{
            //    _surveyAnswerRequest.Criteria.ResposneId = TempData[Epi.Web.Models.Constants.Constant.RESPONSE_ID].ToString();
            //    result = _iSurveyResponseRepository.GetSurveyAnswer(_surveyAnswerRequest).SurveyResponseDTO;
            //}

            return result;
        }


       


    }
}
