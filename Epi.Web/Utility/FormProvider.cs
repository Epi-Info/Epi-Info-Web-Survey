using System.IO;
using System.Linq;
using MvcDynamicForms.Fields;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Web;
using MvcDynamicForms;
using System.Collections.Generic;
using System;
using System.Xml.XPath;
using Epi.Core.EnterInterpreter;
using Epi.Web.Common.DTO;
using Epi.Web.Utility;

namespace Epi.Web.MVC.Utility
{
    public class FormProvider : FormProviderBase
    {
        public static List<Epi.Web.Common.DTO.SurveyAnswerDTO> SurveyAnswerList;
        public static List<Epi.Web.Common.DTO.SurveyInfoDTO> SurveyInfoList;
        public static Form GetForm(SurveyInfoDTO surveyInfo, int pageNumber, SurveyAnswerDTO surveyAnswer, bool isMobile = false, bool IsAndroid = false, List<SourceTableDTO> SourceTableList = null)
            
        {
            Form form = null;
            
            string surveyId = surveyInfo.SurveyId;
            string isMobileText = isMobile ? "true" : "false";

            string cacheKey = surveyId +
                ",page:" + pageNumber.ToString() +
                ",mobile:" + isMobileText;

            bool enableFormCaching = false;

            if (enableFormCaching)
            {
                form = CacheUtility.Get(cacheKey) as Form;
            }

            if (form == null)
            {
                form = new Form();
                form.IsAndroid = IsAndroid;
                if (isMobile)
                {
                    form.IsMobile = isMobile;
                    form.FormWrapperClass = "MvcDynamicMobileForm";
                }

                form.SurveyInfo = surveyInfo;

                if (form.SurveyInfo.IsDraftMode)
                {
                    form.IsDraftModeStyleClass = "draft";
                }
                form.StatusId = surveyAnswer.Status;
                string xml = form.SurveyInfo.XML;
                form.CurrentPage = pageNumber;

                if (string.IsNullOrEmpty(xml))
                {
                    form.NumberOfPages = 1;
                }
                else
                {
                    form.NumberOfPages = GetNumberOfPages(XDocument.Parse(xml));
                }

                if (string.IsNullOrEmpty(xml) == false)
                {
                    form.XDocMetadata = XDocument.Parse(xml);

                    form.FieldsTypeIDs = from _FieldTypeID in form.XDocMetadata.Descendants("Field") select _FieldTypeID;

                    double width, height;
                    width = GetWidth(form.XDocMetadata);
                    height = GetHeight(form.XDocMetadata);
                    form.PageId = GetPageId(form.XDocMetadata, pageNumber);
                    form.Width = width;
                    form.Height = height;

                    XElement ViewElement = form.XDocMetadata.XPathSelectElement("Template/Project/View");
                    string checkcode = ViewElement.Attribute("CheckCode").Value.ToString();
                    StringBuilder JavaScript = new StringBuilder();
                    StringBuilder VariableDefinitions = new StringBuilder();

                    XDocument xdocResponse = XDocument.Parse(surveyAnswer.XML);

                    form.RequiredFieldsList = xdocResponse.Root.Attribute("RequiredFieldsList").Value;
                    form.HiddenFieldsList = xdocResponse.Root.Attribute("HiddenFieldsList").Value;
                    form.HighlightedFieldsList = xdocResponse.Root.Attribute("HighlightedFieldsList").Value;
                    form.DisabledFieldsList = xdocResponse.Root.Attribute("DisabledFieldsList").Value;
                    if (SurveyAnswerList != null)
                    {

                        form.FormCheckCodeObj = form.GetRelateCheckCodeObj(GetRelateFormObj(), checkcode);
                    }
                    else
                    {
                        form.FormCheckCodeObj = form.GetCheckCodeObj(form.XDocMetadata, xdocResponse, checkcode);
                    }
                    form.FormCheckCodeObj.GetVariableJavaScript(VariableDefinitions);
                    form.FormCheckCodeObj.GetSubroutineJavaScript(VariableDefinitions);

                    form.SourceTableList = SourceTableList;

                    string pageName = GetPageName(form.XDocMetadata, pageNumber);

                    JavaScript.Append(GetPageLevelJS(pageNumber, form, pageName, "Before"));
                    JavaScript.Append(GetPageLevelJS(pageNumber, form, pageName, "After"));

                    foreach (var fieldElement in form.FieldsTypeIDs)
                    {
                        string formJavaScript = GetFormJavaScript(checkcode, form, fieldElement.Attribute("Name").Value);
                        JavaScript.Append(formJavaScript);
                    }


                    AddFormFields(pageNumber, form, surveyAnswer, SourceTableList,isMobile);
                    form.FormJavaScript = VariableDefinitions.ToString() + "\n" + JavaScript.ToString();

                }

                if (enableFormCaching)
                {
                    CacheUtility.Insert(cacheKey, form, surveyId);
                }
            }
            else
            {
                form.Fields.Clear();
                AddFormFields(pageNumber, form,surveyAnswer);
            }

            form.ResponseId = surveyAnswer.ResponseId;
         
            form.IsSaved = false;

            if (surveyAnswer.XML.Contains("ResponseDetail"))
            {
                SetStates(form, surveyAnswer);
            }

            return form;
        }

        private static List<Epi.Web.Common.Helper.RelatedFormsObj> GetRelateFormObj()
        {

            List<Epi.Web.Common.Helper.RelatedFormsObj> List = new List<Web.Common.Helper.RelatedFormsObj>();


            for (int i = 0; SurveyAnswerList.Count() > i; i++)
            {


                Epi.Web.Common.Helper.RelatedFormsObj RelatedFormsObj = new Epi.Web.Common.Helper.RelatedFormsObj();
                XDocument xdocResponse1 = XDocument.Parse(SurveyAnswerList[i].XML);
                XDocument xdoc1 = XDocument.Parse(SurveyInfoList[i].XML.ToString());
                RelatedFormsObj.MetaData = xdoc1;
                RelatedFormsObj.Response = xdocResponse1;


                List.Add(RelatedFormsObj);
            }

            return List;
        }
    }
}
