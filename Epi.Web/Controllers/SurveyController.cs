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
        public ActionResult Index(string responseId, int PageNumber = 1)
        {

            //if (!string.IsNullOrEmpty(page))
            //{

            
            //get the survey form
            try
            {
                Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO = GetSurveyAnswer(responseId);

                var form = _isurveyFacade.GetSurveyFormData(surveyAnswerDTO.SurveyId, PageNumber, surveyAnswerDTO);

                ////create the responseid
                //Guid ResponseID = Guid.NewGuid();

                ////put the ResponseId in Temp data for later use
                //TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = ResponseID.ToString();
               
                //// create the first survey response
                //_isurveyFacade.CreateSurveyAnswer(surveyId, ResponseID.ToString());
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
        //public ActionResult Index(SurveyInfoModel surveyInfoModel, string Submitbutton, string Savebutton, string ContinueButton, string PreviousButton, int PageNumber = 1)
        public ActionResult Index(SurveyAnswerModel surveyAnswerModel, string Submitbutton, string Savebutton, string ContinueButton, string PreviousButton, int PageNumber = 1)
        {

            string responseId = surveyAnswerModel.ResponseId;
            try
            {
                Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];

                SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyAnswer.SurveyId);
                
                //get the survey form
                MvcDynamicForms.Form form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, this.GetCurrentPage() == 0 ? 1 : this.GetCurrentPage(), SurveyAnswer);
                //Update the model
                UpdateModel(form);
                bool IsSubmited = false;
                bool IsSaved = false;
                if (form.Validate())
                {
                   
                    if (!string.IsNullOrEmpty(Submitbutton))
                    {
                        IsSubmited = true;//survey has been submited
                    }
                    if (!string.IsNullOrEmpty(Savebutton))
                    {
                        IsSaved = true;//survey has been submited
                    }
                    _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form, SurveyAnswer, IsSubmited, IsSaved);
                    
                    if (!string.IsNullOrEmpty(Submitbutton))
                    {

                        for (int i = 1; i < form.NumberOfPages; i++)
                        {
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, i, SurveyAnswer);
                            if (!form.Validate())
                            {
                                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                            }
                        }


                        int invalidPage = Epi.Web.Utility.ValidateResponse.Validate(form.SurveyInfo.XML, SurveyAnswer.XML);  
                        if(invalidPage > 0)
                        {
                            form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, invalidPage, SurveyAnswer);
                            return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Final", new { surveyId = surveyInfoModel.SurveyId });
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
                        form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, PageNumber, SurveyAnswer);

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

        private Epi.Web.Common.DTO.SurveyAnswerDTO GetSurveyAnswer(string responseId)
        {
            Epi.Web.Common.DTO.SurveyAnswerDTO result = null;

            //responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
            result =  _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];

            return result;

        }





    }
}
