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
using System.Web;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.Text;
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
        public static Epi.Web.Common.DTO.SurveyAnswerDTO CreateSurveyResponse(
            string surveyId, 
            string responseId, 
            SurveyAnswerRequest surveyAnswerRequest,
            Common.DTO.SurveyAnswerDTO surveyAnswerDTO,
            SurveyResponseXML surveyResponseXML, 
            ISurveyAnswerRepository iSurveyAnswerRepository)
        {
            bool AddRoot = false;
            surveyAnswerRequest.Criteria.SurveyAnswerIdList.Add(responseId.ToString());
            surveyAnswerDTO.ResponseId = responseId.ToString();
            //surveyAnswerDTO.DateCompleted = DateTime.Now;
            surveyAnswerDTO.DateCreated = DateTime.Now;
            surveyAnswerDTO.SurveyId = surveyId;
            surveyAnswerDTO.Status = (int)Constant.Status.InProgress;
            surveyAnswerDTO.XML = surveyResponseXML.CreateResponseXml(surveyId, AddRoot,0,"").InnerXml;
            surveyAnswerRequest.SurveyAnswerList.Add(surveyAnswerDTO);
            surveyAnswerRequest.Action = Epi.Web.MVC.Constants.Constant.CREATE;  //"Create";
            iSurveyAnswerRepository.SaveSurveyAnswer(surveyAnswerRequest);

            return surveyAnswerDTO;
        }

        public static void UpdateSurveyResponse(
            SurveyInfoModel surveyInfoModel,
            MvcDynamicForms.Form form, 
            SurveyAnswerRequest surveyAnswerRequest,
            SurveyResponseXML surveyResponseXML,
            ISurveyAnswerRepository iSurveyAnswerRepository,
            SurveyAnswerResponse surveyAnswerResponse, 
            string responseId, 
            Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO, 
            bool IsSubmited, 
            bool IsSaved,
            int  PageNumber)
        {
            // 1 Get the record for the current survey response
            // 2 update the current survey response
            // 3 save the current survey response
            

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
               if (form.RequiredFieldsList != null)
               {

                   Xdoc.Root.Attribute("RequiredFieldsList").Value = "";
                   Xdoc.Root.Attribute("RequiredFieldsList").Value = form.RequiredFieldsList.ToString();

               }
               if (surveyAnswerDTO.RecordBeforeFlag)
                   {
                   Xdoc.Root.Attribute("RecordBeforeFlag").Value = "True";
                
                   }
               //  AssignList 
               List<KeyValuePair<string, String>> FieldsList = new List<KeyValuePair<string, string>>();
              
               FieldsList = GetHiddenFieldsList(form);
               if (FieldsList != null)
               {
                    IEnumerable <XElement> XElementList = Xdoc.XPathSelectElements("SurveyResponse/Page/ResponseDetail");
                    for (var i = 0; i < FieldsList.Count; i++)
                    {
                        foreach (XElement Element in XElementList)
                        {
                            if (Element.Attribute("QuestionName").Value.ToString().Equals(FieldsList[i].Key, StringComparison.OrdinalIgnoreCase))
                            {
                                if (FieldsList[i].Value != null)
                                {
                                    Element.Value = FieldsList[i].Value;
                                }
                                break;
                            }
                        }
                    }
               }

                       
 
             
            ////Update survey response Status
            if (IsSubmited)
            {

                surveyAnswerRequest.SurveyAnswerList[0].Status = 3;
                surveyAnswerRequest.SurveyAnswerList[0].DateCompleted = DateTime.Now;
                Xdoc.Root.Attribute("LastPageVisited").Remove();
                Xdoc.Root.Attribute("HiddenFieldsList").Remove();
                Xdoc.Root.Attribute("HighlightedFieldsList").Remove();
                Xdoc.Root.Attribute("DisabledFieldsList").Remove();
                Xdoc.Root.Attribute("RequiredFieldsList").Remove();
                Xdoc.Root.Attribute("RecordBeforeFlag").Remove();
                //RemovePageNumAtt(Xdoc);
            }
            if (IsSaved)
            {
                surveyAnswerRequest.SurveyAnswerList[0].Status = 2;
            }
            surveyAnswerRequest.SurveyAnswerList[0].XML = Xdoc.ToString();
            /////Update Survey Mode ////////////////////
            surveyAnswerRequest.SurveyAnswerList[0].IsDraftMode = surveyAnswerDTO.IsDraftMode;
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
        ///   This function will loop through the form controls and checks if any of the controls are found in the context detail list. 
        ///   If any their values get updated from the context list.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ContextDetailList"></param>
        /// <returns>Returns a Form object</returns>
        public static MvcDynamicForms.Form UpdateControlsValuesFromContext(MvcDynamicForms.Form form, Dictionary<string, string> ContextDetailList)
        {



            Dictionary<string, string> formControlList = new Dictionary<string, string>();

            //var responses = new List<Response>();
            foreach (var field in form.InputFields)
            {
                string fieldName = field.Title;

                if (ContextDetailList.ContainsKey(fieldName))
                {
                    field.Response = ContextDetailList[fieldName].ToString();
                }

            }



            return form;
        }
        public static MvcDynamicForms.Form UpdateControlsValues (MvcDynamicForms.Form form, string Name , string Value)
        {
 
            foreach (var field in form.InputFields)
            {
                string fieldName = field.Title;

                if (Name.ToLower() == fieldName.ToLower())
                {
                    field.Response = Value.ToString();
                }

            }



            return form;
        }
        public static Dictionary<string, string> GetContextDetailList(Epi.Core.EnterInterpreter.EnterRule FunctionObject)
        {


            Dictionary<string, string> ContextDetailList = new Dictionary<string, string>();


            if (FunctionObject != null && !FunctionObject.IsNull())
            {

                foreach (KeyValuePair<string, EpiInfo.Plugin.IVariable> kvp in FunctionObject.Context.CurrentScope.SymbolList)
                {
                    EpiInfo.Plugin.IVariable field = kvp.Value;

                    if (!string.IsNullOrEmpty(field.Expression))
                    {
                        if (field.DataType == EpiInfo.Plugin.DataType.Date)
                        {
        
                            var  datetemp =  string.Format("{0:MM/dd/yyyy}", field.Expression);
                            DateTime date = new DateTime();
                            date = Convert.ToDateTime(datetemp);
                            ContextDetailList[kvp.Key] = date.Date.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            ContextDetailList[kvp.Key] = field.Expression;
                        }
                    }
                }
            }


            return ContextDetailList;
        }

        public static List<KeyValuePair<string, string>> GetHiddenFieldsList(MvcDynamicForms.Form pForm)
        {

            List<KeyValuePair<string, String>> FieldsList = new List<KeyValuePair<string, string>>();

            foreach (var field in pForm.InputFields)
            {
                if (field.IsPlaceHolder)
                {
                    FieldsList.Add(new KeyValuePair<string, string>(field.Title, field.Response));
                     
                }
            }

            return FieldsList;
        }
        public static void UpdatePassCode(UserAuthenticationRequest AuthenticationRequest, ISurveyAnswerRepository iSurveyAnswerRepository)
        {


            iSurveyAnswerRepository.UpdatePassCode(AuthenticationRequest);

        }
        public static string GetPassCode() {

            Guid Guid = Guid.NewGuid();
            string Passcode = Guid.ToString().Substring(0, 4);
            return Passcode;
        }
        public static bool IsGuid(string expression)
        {
            if (expression != null)
            {
                Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

                return guidRegEx.IsMatch(expression);
            }
            return false;
        }



        public static int GetNumberOfPags(string ResponseXml)
        {

            XDocument xdoc = XDocument.Parse(ResponseXml);
            int PageNumber = 0;
            PageNumber = xdoc.Root.Elements("Page").Count();
  
            return PageNumber;


        }

        public static bool IsMobileDevice(string RequestUserAgent)
        {


            if (RequestUserAgent.IndexOf("Opera Mobi", StringComparison.OrdinalIgnoreCase) >= 0 || RequestUserAgent.Contains("Opera Mobi"))
            {
                return true;
            }
            else if (RequestUserAgent.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0 || RequestUserAgent.Contains("Android"))
            {
                return true;
            }
            else if (RequestUserAgent.IndexOf("Mobile", StringComparison.OrdinalIgnoreCase) >= 0 || RequestUserAgent.Contains("Mobile"))
            {
                return true;
            }
            else if (RequestUserAgent.IndexOf("Phone", StringComparison.OrdinalIgnoreCase) >= 0 || RequestUserAgent.Contains("Phone"))
            {
                return true;
            }
            else if (RequestUserAgent.IndexOf("Opera Mini", StringComparison.OrdinalIgnoreCase) >= 0 || RequestUserAgent.Contains("Opera Mini"))
            {
                return true;
            }
            else 
            {

                return false;
            }
        
        }


        internal static List<PrintModel> GetQuestionAnswerList(string ResponseXml, SurveyControlsResponse List)
            {

            List<PrintModel> QuestionAnswerList = new List<PrintModel>();
            int NumberOfPages = GetNumberOfPags(ResponseXml);


            XDocument xdoc = XDocument.Parse(ResponseXml);


            

            for (int i=1; NumberOfPages + 1 > i  ;i++ )
                {

                var _FieldsTypeIDs = from _FieldTypeID in
                                     xdoc.Descendants("Page")
                                    
                                     where _FieldTypeID.Attribute("PageNumber").Value == (i).ToString()
                                     select _FieldTypeID;

                var _PageFieldsTypeIDs = from _FieldTypeID1 in
                                             _FieldsTypeIDs.Descendants("ResponseDetail")
                                         
                                         select _FieldTypeID1;


                foreach (var item in _PageFieldsTypeIDs)
                    {

                    string ControlId  = item.Attribute("QuestionName").Value;
                    bool Type = (bool) List.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType != "Literal" && x.ControlType != "GroupBox");
                    bool YesNoType = (bool)List.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType == "YesNo");

                    if (Type)
                        {
                    string Question = List.SurveyControlList.Single(x => x.ControlId == ControlId).ControlPrompt;
                    string ControlType = List.SurveyControlList.Single(x => x.ControlId == ControlId).ControlType;
                    PrintModel PrintModel = new PrintModel();

                    PrintModel.PageNumber = i;
                    PrintModel.Question = Question;
                    PrintModel.ControlName = ControlId;
                    PrintModel.ControlType =  ControlType;
                    if (!YesNoType)
                        {
                            PrintModel.Value = item.Value;
                        }
                    else{
                    if (item.Value == "1")
                                {
                                PrintModel.Value = "Yes";
                                }
                            else
                                {

                                PrintModel.Value = "No";
                                }
                            
                        }
                    QuestionAnswerList.Add(PrintModel);
                        }
                    }

                }



            return QuestionAnswerList;

            }

        internal static List<PrintModel> SetCommentLegalValues(List<PrintModel> QuestionAnswerList, SurveyControlsResponse List, SurveyInfoModel surveyInfoModel)
        {
            try
            {
                XDocument xdoc = XDocument.Parse(surveyInfoModel.XML);
                var CommentLegals = List.SurveyControlList.Where(x => x.ControlType == "CommentLegal");
                if (CommentLegals.Count() > 0)
                {
                    foreach (var item in CommentLegals)
                    {
                        List<string> CommentLegalValues = GetCommentLegalValues(xdoc, item.ControlId);
                        PrintModel AnswerValue = QuestionAnswerList.Where(x => x.ControlName == item.ControlId).Single();
                        QuestionAnswerList = SetValues(CommentLegalValues, AnswerValue, QuestionAnswerList);
                    }


                }
            }
            catch (Exception ex){
                throw ex;
            }

            return QuestionAnswerList;
        }

        private static List<PrintModel> SetValues(List<string> CommentLegalValues, PrintModel AnswerValue, List<PrintModel> QuestionAnswerList)
        {
            if (!string.IsNullOrEmpty(AnswerValue.Value))
            {
            string NewValue = CommentLegalValues.Where(x => x.StartsWith(AnswerValue.Value + "-")).Single();
            NewValue = NewValue.Split(new char[] { '-' }, 2)[1]; // Split(new char[] { '-' }, 2)
            QuestionAnswerList.Remove(AnswerValue);
            AnswerValue.Value = NewValue;
            }
            QuestionAnswerList.Add(AnswerValue);

            return QuestionAnswerList;
        }
        public static List<string> GetCommentLegalValues(XDocument xdocMetadata, string Name)
        {
            List<string> commentLegal = new List<string>();
           
            var fieldElementList = from fieldElements in xdocMetadata.Descendants("Field")
                                   where fieldElements.Attribute("Name").Value == Name
                                   select fieldElements;

            foreach (var fieldElement in fieldElementList)
            { 
                              commentLegal =  GetValuesList(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value);
                            break;
            }
            return commentLegal;
        }

        private static List<string> GetValuesList(XDocument xdoc, string ControlName, string TableName)
        {
            StringBuilder DropDownValues = new StringBuilder();

            if (!string.IsNullOrEmpty(xdoc.ToString()))
            {
                var _ControlValues = from _ControlValue in xdoc.Descendants("SourceTable")
                                     where _ControlValue.Attribute("TableName").Value == TableName.ToString()
                                     select _ControlValue;

                foreach (var _ControlValue in _ControlValues)
                {
                    var _SourceTableValues = from _SourceTableValue in _ControlValues.Descendants("Item")
                                             select _SourceTableValue;

                    foreach (var _SourceTableValue in _SourceTableValues)
                    {
                        DropDownValues.Append(_SourceTableValue.FirstAttribute.Value.Trim());
                        DropDownValues.Append("&#;");
                    }
                }
            }

            return DropDownValues.ToString().Split("&#;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();
        }


    }
}