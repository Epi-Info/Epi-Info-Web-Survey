using System;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Constants;
using Epi.Web.MVC.Utility;
using Epi.Web.MVC.Models;
using Epi.Web.MVC.Facade;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Xml.XPath;
namespace Epi.Web.MVC.Utility
{
    public class SurveyHelper
    {
        /// <summary>
        /// Creates the first survey response in the response table
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="responseId"></param>
        /// <param name="surveyAnswerRequest"></param>
        /// <param name="surveyAnswerDTO"></param>
        /// <param name="surveyResponseXML"></param>
        /// <param name="iSurveyAnswerRepository"></param>
        public static Epi.Web.Common.DTO.SurveyAnswerDTO CreateSurveyResponse(string surveyId, string responseId, SurveyAnswerRequest surveyAnswerRequest,
                                          Common.DTO.SurveyAnswerDTO surveyAnswerDTO,
                                          SurveyResponseXML surveyResponseXML, ISurveyAnswerRepository iSurveyAnswerRepository)
        {
            bool AddRoot = false;
            surveyAnswerRequest.Criteria.SurveyAnswerIdList.Add(responseId.ToString());
            surveyAnswerDTO.ResponseId = responseId.ToString();
            surveyAnswerDTO.DateCompleted = DateTime.Now;
            surveyAnswerDTO.SurveyId = surveyId;
            surveyAnswerDTO.Status = (int)Constant.Status.InProgress;
            surveyAnswerDTO.XML = surveyResponseXML.CreateResponseXml(surveyId, AddRoot,0,"").InnerXml;
            surveyAnswerRequest.SurveyAnswerList.Add(surveyAnswerDTO);
            surveyAnswerRequest.Action = Epi.Web.MVC.Constants.Constant.CREATE;  //"Create";
            iSurveyAnswerRepository.SaveSurveyAnswer(surveyAnswerRequest);

            return surveyAnswerDTO;
        }

        public static void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel,MvcDynamicForms.Form form, SurveyAnswerRequest surveyAnswerRequest,
                                                             SurveyResponseXML surveyResponseXML,
                                                            ISurveyAnswerRepository iSurveyAnswerRepository,
                                                             SurveyAnswerResponse surveyAnswerResponse, string responseId, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, bool IsSubmited, bool IsSaved,int  PageNumber)
        {
            // 1 Get the record for the current survey response
            // 2 update the current survey response
            // 3 save the current survey response
            if (!IsSubmited)
            {

                // 2 a. update the current survey answer request
                surveyAnswerRequest.SurveyAnswerList = surveyAnswerResponse.SurveyResponseList;
                surveyResponseXML.Add(form);
                XDocument SavedXml = XDocument.Parse(surveyAnswerDTO.XML);
                bool AddRoot = false;
                if (SavedXml.Root.FirstAttribute.Value.ToString() == "0")
                {
                    AddRoot = true;
                }
                surveyAnswerRequest.SurveyAnswerList[0].XML = surveyResponseXML.CreateResponseXml(surveyInfoModel.SurveyId, AddRoot, form.CurrentPage,form.PageId).InnerXml;
                // 2 b. save the current survey response
                surveyAnswerRequest.Action = Epi.Web.MVC.Constants.Constant.UPDATE;  //"Update";
                //Append to Response Xml

                XDocument CurrentPageResponseXml = XDocument.Parse(surveyAnswerRequest.SurveyAnswerList[0].XML);
                if (SavedXml.Root.FirstAttribute.Value.ToString() != "0")
                {
                    surveyAnswerRequest.SurveyAnswerList[0].XML = MergeXml(SavedXml, CurrentPageResponseXml, form.CurrentPage).ToString();
                }
            }
                ////Update page number before saving response XML

                XDocument Xdoc = XDocument.Parse(surveyAnswerRequest.SurveyAnswerList[0].XML);
                if (PageNumber != 0)
                {
                    Xdoc.Root.Attribute("LastPageVisited").Value = PageNumber.ToString();
                }
                ////Update Hidden Fields List before saving response XML
               if (form.HiddenFieldsList !=null)
                {
                     
                        Xdoc.Root.Attribute("HiddenFieldsList").Value = "";
                        Xdoc.Root.Attribute("HiddenFieldsList").Value = form.HiddenFieldsList.ToString();
                   
                }
               if (form.HighlightedFieldsList != null)
               {

                   Xdoc.Root.Attribute("HighlightedFieldsList").Value = "";
                   Xdoc.Root.Attribute("HighlightedFieldsList").Value = form.HighlightedFieldsList.ToString();

               }
               if (form.DisabledFieldsList != null)
               {

                   Xdoc.Root.Attribute("DisabledFieldsList").Value = "";
                   Xdoc.Root.Attribute("DisabledFieldsList").Value = form.DisabledFieldsList.ToString();

               }
           
            ////Update survey response Status
            if (IsSubmited)
            {

                surveyAnswerRequest.SurveyAnswerList[0].Status = 3;
                Xdoc.Root.Attribute("LastPageVisited").Remove();
                Xdoc.Root.Attribute("HiddenFieldsList").Remove();
                Xdoc.Root.Attribute("HighlightedFieldsList").Remove();
                Xdoc.Root.Attribute("DisabledFieldsList").Remove();
                RemovePageNumAtt(Xdoc);
            }
            if (IsSaved)
            {

                surveyAnswerRequest.SurveyAnswerList[0].Status = 2;

            }
            surveyAnswerRequest.SurveyAnswerList[0].XML = Xdoc.ToString();
            iSurveyAnswerRepository.SaveSurveyAnswer(surveyAnswerRequest);
           

           
        }
        //Remove PageNumber attribute
        private static void RemovePageNumAtt(XDocument Xdoc)
        {
            var _Pages = from _Page in Xdoc.Descendants("Page") select _Page;

           foreach (var _Page in _Pages) 
            {

                _Page.Attribute("PageNumber").Remove();
            }


             
        }



      

        /// <summary>
        /// Returns a SurveyInfoDTO object
        /// </summary>
        /// <param name="surveyInfoRequest"></param>
        /// <param name="iSurveyInfoRepository"></param>
        /// <param name="SurveyId"></param>
        /// <returns></returns>
        public static Epi.Web.Common.DTO.SurveyInfoDTO GetSurveyInfoDTO(SurveyInfoRequest surveyInfoRequest,
                                                  ISurveyInfoRepository iSurveyInfoRepository,                 
                                                  string SurveyId)
        {
            surveyInfoRequest.Criteria.SurveyIdList.Add(SurveyId);
            return iSurveyInfoRepository.GetSurveyInfo(surveyInfoRequest).SurveyInfoList[0];
        }

        public static XDocument MergeXml(XDocument SavedXml, XDocument CurrentPageResponseXml, int Pagenumber)
        {

            XDocument xdoc = XDocument.Parse(SavedXml.ToString());
            XElement oldXElement = xdoc.XPathSelectElement("SurveyResponse/Page[@PageNumber = '" + Pagenumber.ToString() + "']");
 

            if (oldXElement == null)
            {
                SavedXml.Root.Add(CurrentPageResponseXml.Elements());
                return SavedXml;
            }

            else 
            {
                oldXElement.Remove();
                xdoc.Root.Add(CurrentPageResponseXml.Elements());
                return xdoc;
            }
            
        
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ContextDetailList"></param>
        /// <returns>Returns a Form object</returns>
        public static MvcDynamicForms.Form UpdateControlsValuesFromContext(MvcDynamicForms.Form form, Dictionary<string, string> ContextDetailList)
        {



            Dictionary<string, string> formControlList = new Dictionary<string, string>();

            //var responses = new List<Response>();
            foreach (var field in form.InputFields.OrderBy(x => x.DisplayOrder))
            {
                string fieldName = field.Title;

                //  formControlList[field.Title] =   field.GetType().ToString().Substring(23);
                for (int i = 0; i < ContextDetailList.Count(); i++)
                {

                    if (ContextDetailList.ContainsKey(fieldName))
                    {
                        field.Response = ContextDetailList[fieldName].ToString();
                    }


                }

            }



            return form;
        }

        public static Dictionary<string, string> GetContextDetailList(Epi.Core.EnterInterpreter.EnterRule FunctionObject)
        {


            Dictionary<string, string> ContextDetailList = new Dictionary<string, string>();


            if (FunctionObject != null && !FunctionObject.IsNull())
            {

                foreach (var field in FunctionObject.Context.CurrentScope.SymbolList)
                {
                    if (!string.IsNullOrEmpty(field.Value.Expression))
                    {
                        ContextDetailList[field.Key] = field.Value.Expression;
                    }
                }
            }


            return ContextDetailList;
        }


    }
}