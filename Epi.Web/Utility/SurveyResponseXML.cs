using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Epi.Web.Common.Message;

namespace Epi.Web.MVC.Utility
{
    public class SurveyResponseXML
    {

        Dictionary<string, string> ResponseDetailList = new Dictionary<string, string>();
        

        public void Add(MvcDynamicForms.Form pForm)
        {
            ResponseDetailList.Clear();
            foreach (var field in pForm.InputFields)
            {
                if (!field.IsPlaceHolder)
                {
                    if (this.ResponseDetailList.ContainsKey(field.Title))
                    {
                        this.ResponseDetailList[field.Title] = field.Response;
                    }
                    else
                    {
                        this.ResponseDetailList.Add(field.Title, field.Response);
                    }
                }
            }
        }

        public void Add(MvcDynamicForms.Fields.InputField pField)
        {
            if (this.ResponseDetailList.ContainsKey(pField.Title))
            {
                this.ResponseDetailList[pField.Title] = pField.GetXML();
            }
            else
            {
                this.ResponseDetailList.Add(pField.Title, pField.GetXML());
            }
        }

         public void SetValue(string pKey, string pXMLValue)
         {
            if(this.ResponseDetailList.ContainsKey(pKey))
            {
                this.ResponseDetailList[pKey] = pXMLValue;
            }
            else
            {
                this.ResponseDetailList.Add(pKey, pXMLValue);
            }
         }


          public string GetValue(string pKey)
         {
             string result = null;

            if(this.ResponseDetailList.ContainsKey(pKey))
            {
               result = this.ResponseDetailList[pKey];
            }

            return result;
         }

          public XmlDocument  CreateResponseXml(string SurveyId, bool AddRoot, int CurrentPage,string Pageid)
          {

               

              
              XmlDocument xml = new XmlDocument();
              XmlElement root = xml.CreateElement("SurveyResponse");
             
              if ( CurrentPage==0)
              {

              root.SetAttribute("SurveyId", SurveyId);
              root.SetAttribute("LastPageVisited", "1");
              root.SetAttribute("HiddenFieldsList","");
              root.SetAttribute("HighlightedFieldsList", "");
              root.SetAttribute("DisabledFieldsList", "");
              root.SetAttribute("RequiredFieldsList", "");
              root.SetAttribute("RecordBeforeFlag", "");
              xml.AppendChild(root);
              }

              XmlElement PageRoot = xml.CreateElement("Page");
              if ( CurrentPage!=0)
              {
               
                 
                  PageRoot.SetAttribute("PageNumber", CurrentPage.ToString());                
                  PageRoot.SetAttribute("PageId", Pageid);//Added PageId Attribute to the page node
                  PageRoot.SetAttribute("MetaDataPageId", Pageid.ToString());
                  xml.AppendChild(PageRoot);
              }

              foreach ( KeyValuePair<string, string> pair  in this.ResponseDetailList)
              {
                  XmlElement child = xml.CreateElement(Epi.Web.MVC.Constants.Constant.RESPONSE_DETAILS);
                  child.SetAttribute("QuestionName", pair.Key);
                  child.InnerText = pair.Value;
                  PageRoot.AppendChild(child);
              }

            
              return xml;
          }


          

    }
}
