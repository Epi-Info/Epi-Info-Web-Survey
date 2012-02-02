using System;
using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using System.Collections.Generic;
using System.Text;
namespace Epi.Web.MVC.Controllers
{
    public class SurveyController : Controller
    {


        

        //declare SurveyTransactionObject object
        private ISurveyFacade _isurveyFacade;
        /// <summary>
        /// Injectinting SurveyTransactionObject through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        
        public SurveyController(ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
        
        
                
       
       /// <summary>
       /// create the new resposeid and put it in temp data. create the form object. create the first survey response
       /// </summary>
       /// <param name="surveyId"></param>
       /// <returns></returns>
 
        [HttpGet]
        public ActionResult Index(string surveyId, int PageNumber = 1)
        {

            //if (!string.IsNullOrEmpty(page))
            //{

            
            //get the survey form
            try
            {

                var form = _isurveyFacade.GetSurveyFormData(surveyId,PageNumber, this.GetCurrentSurveyAnswer());

                //create the responseid
                Guid ResponseID = Guid.NewGuid();

                //put the ResponseId in Temp data for later use
                TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = ResponseID.ToString();
               
                // create the first survey response
                _isurveyFacade.CreateSurveyAnswer(surveyId, ResponseID.ToString());
                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
            }
            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
            //}
            //return null;
        }
        [HttpPost] [ValidateAntiForgeryToken]
        public ActionResult Index(SurveyInfoModel surveyInfoModel, string Submitbutton, string Savebutton, string ContinueButton, string PreviousButton, int PageNumber = 1)
        {

            string responseId = null;
            try
            {
            
                //get the survey form
                MvcDynamicForms.Form form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, this.GetCurrentPage()== 0? 1 : this.GetCurrentPage(), this.GetCurrentSurveyAnswer());
                //Update the model
                UpdateModel(form);

                if (form.Validate())
                {

                    if (TempData.ContainsKey(Epi.Web.MVC.Constants.Constant.RESPONSE_ID))
                    {
                        if (!string.IsNullOrEmpty(TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString()))
                        {
                            responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
                            _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, this.GetCurrentSurveyAnswer());
                        }
                    }

                    if (!string.IsNullOrEmpty(Submitbutton))
                    {


                        int invalidPage = Epi.Web.Utility.ValidateResponse.Validate(form.SurveyInfo.XML, this.GetCurrentSurveyAnswer().XML);  
                        if(invalidPage > 0)
                        {
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, invalidPage, this.GetCurrentSurveyAnswer());
                            return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                        }
                        else
                        {

                            TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = null;
                            return RedirectToAction("Index", "Final");
                        }
                    }
                    else if (!string.IsNullOrEmpty(Savebutton))
                    {
                        form.SaveClicked = true;
                        return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);

                    }



                    //else if (!string.IsNullOrEmpty(ContinueButton))
                    //{
                    //    form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, PageNumber + 1, this.GetCurrentSurveyAnswer());
                    //    return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                    //}
                    //else if (!string.IsNullOrEmpty(PreviousButton))
                    //{
                    //    form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, PageNumber - 1, this.GetCurrentSurveyAnswer());
                    //    return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                    //}  
                    else
                    {
                        //goto url
                        form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, PageNumber, this.GetCurrentSurveyAnswer());

                        //form.Validate();

                        return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                     }

                }
                else
                {
                    //stay on same page
                    return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                }



            }
            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
        }
      


        



        
        private int GetCurrentPage()
        {
            int CurrentPage = 1;
            
            string PageNum = this.Request.UrlReferrer.ToString().Substring(this.Request.UrlReferrer.ToString().LastIndexOf('/')+1);

            int.TryParse(PageNum, out CurrentPage);
            return CurrentPage;
        }

        private Epi.Web.Common.DTO.SurveyAnswerDTO GetCurrentSurveyAnswer()
        {
            Epi.Web.Common.DTO.SurveyAnswerDTO result = null;

            if (TempData.ContainsKey(Epi.Web.MVC.Constants.Constant.RESPONSE_ID) && TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] != null
                                                                               && !string.IsNullOrEmpty(TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString()))
            {
               
                string responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();

               
                   //TODO: Now repopulating the TempData (by reassigning to responseId) so it persisits, later we will need to find a better 
                   //way to replace it. 
                    TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = responseId;
                    return _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
               


            }

            return result;
        }





    }
}
