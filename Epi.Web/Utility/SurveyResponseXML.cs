using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Epi.Web.MVC.Utility
{
    public class SurveyResponseXML
    {

        Dictionary<string, string> ResponseDetailList = new Dictionary<string, string>();
        

        public void Add(MvcDynamicForms.Form pForm)
        {

            foreach (var field in pForm.InputFields)
            {
           
                if(this.ResponseDetailList.ContainsKey(field.Key))
                {
                    this.ResponseDetailList[field.Title] = field.Key;
                }
                else
                {
                    this.ResponseDetailList.Add(field.Title, field.Response);
                }
            }
        }

        public void Add(MvcDynamicForms.Fields.InputField pField)
        {
            if(this.ResponseDetailList.ContainsKey(pField.Key))
            {
                this.ResponseDetailList[pField.Title] = pField.Response;
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

          public XmlDocument CreateResponseXml(string SurveyId)
          {


              XmlDocument xml = new XmlDocument();
              XmlElement root = xml.CreateElement(Epi.Web.MVC.Constants.Constant.SURVEY_RESPONSE);
              root.SetAttribute(Epi.Web.MVC.Constants.Constant.SURVEY_ID, SurveyId);
              xml.AppendChild(root);


              foreach ( KeyValuePair<string, string> pair  in this.ResponseDetailList)
              {
                  XmlElement child = xml.CreateElement(Epi.Web.MVC.Constants.Constant.RESPONSE_DETAILS);
                  child.SetAttribute("QuestionName", pair.Key);
                  child.InnerText = pair.Value;
                  root.AppendChild(child);
              }


              return xml;
          }

    }
}