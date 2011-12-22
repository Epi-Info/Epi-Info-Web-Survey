using System;
using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
namespace Epi.Web.MVC.Controllers
{
    public class SurveyController : Controller
    {


        

        //declare SurveyTransactionObject object
        private SurveyFacade _surveyFacade;
        /// <summary>
        /// Injectinting SurveyTransactionObject through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        
        public SurveyController(SurveyFacade surveyFacade)
        {
            _surveyFacade = surveyFacade;
        }
        
        
                
       
       /// <summary>
       /// create the new resposeid and put it in temp data. create the form object. create the first survey response
       /// </summary>
       /// <param name="surveyId"></param>
       /// <returns></returns>
 
        
        public ActionResult Index(string surveyId,string page)
        {

            //if (!string.IsNullOrEmpty(page))
            //{
               
                //get the survey form
                var form = _surveyFacade.GetSurveyFormData(surveyId, this.GetCurrentPage(), this.GetCurrentSurveyAnswer());

                //create the responseid
                Guid ResponseID = Guid.NewGuid();
               
                //put the ResponseId in Temp data for later use
                TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = ResponseID.ToString();

                // create the first survey response
                _surveyFacade.CreateSurveyAnswer(surveyId, ResponseID.ToString());
                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
            //}
            //return null;
        }
        [HttpPost]
        public ActionResult Index(SurveyInfoModel surveyInfoModel,string Submit)
        {
            
            //get the survey form
            MvcDynamicForms.Form form = _surveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, this.GetCurrentPage(), this.GetCurrentSurveyAnswer());
            //Update the model
            UpdateModel(form);
            if (form.Validate())
            {
                _surveyFacade.UpdateSurveyResponse(surveyInfoModel, TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString(), form);

                //return RedirectToAction("Index", "Final", new {id="final" });
                //return RedirectToAction("Index", "Survey");
               return RedirectToAction("Index", "Final");
            }  
            else
            {
                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
            }

            
        }
      


        



        
        private int GetCurrentPage()
        {
            int CurrentPage = 1;
            if (TempData.ContainsKey(Epi.Web.MVC.Constants.Constant.CURRENT_PAGE) && TempData[Epi.Web.MVC.Constants.Constant.CURRENT_PAGE] != null)
            {
                int.TryParse(TempData[Epi.Web.MVC.Constants.Constant.CURRENT_PAGE].ToString(), out CurrentPage);
            }

            return CurrentPage;
        }

        private Epi.Web.Common.DTO.SurveyAnswerDTO GetCurrentSurveyAnswer()
        {
            Epi.Web.Common.DTO.SurveyAnswerDTO result = null;

            if (TempData.ContainsKey(Epi.Web.MVC.Constants.Constant.RESPONSE_ID) && TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] != null
                                                                               && !string.IsNullOrEmpty(TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString()))
            {
                //_surveyAnswerRequest.Criteria.ResposneId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
                //result = _iSurveyResponseRepository.GetSurveyAnswer(_surveyAnswerRequest).SurveyResponseDTO;
                return _surveyFacade.GetSurveyAnswerResponse(TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString()).SurveyResponseDTO;
                 
            }

            return result;
        }


       


    }
}
