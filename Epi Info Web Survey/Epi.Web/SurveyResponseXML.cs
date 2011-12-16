using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Epi.Web
{
    public class SurveyResponseXML
    {

        Dictionary<string, string> ResponseDetailList = new Dictionary<string, string>();
        string _SurveyId;

        public SurveyResponseXML(string pSurveyId)
        {
            this._SurveyId = pSurveyId;
        }

        public void Add(MvcDynamicForms.Form pForm)
        {

            foreach (var field in pForm.InputFields)
            {
           
                if(this.ResponseDetailList.ContainsKey(field.Key))
                {
                    this.ResponseDetailList[field.Key] = field.Key;
                }
                else
                {
                    this.ResponseDetailList.Add(field.Key, field.Response);
                }
            }
        }

        public void Add(MvcDynamicForms.Fields.InputField pField)
        {
            if(this.ResponseDetailList.ContainsKey(pField.Key))
            {
                this.ResponseDetailList[pField.Key] = pField.Response;
            }
            else
            {
                this.ResponseDetailList.Add(pField.Key, pField.GetXML());
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

          public XmlDocument CreateResponseXml()
          {


              XmlDocument xml = new XmlDocument();
              XmlElement root = xml.CreateElement("SurveyResponse");
              root.SetAttribute("SurveyId", this._SurveyId);
              xml.AppendChild(root);


              foreach ( KeyValuePair<string, string> pair  in this.ResponseDetailList)
              {
                  XmlElement child = xml.CreateElement("ResponseDetail");
                  child.SetAttribute("QuestionId", pair.Key);
                  child.InnerText = pair.Value;
                  root.AppendChild(child);
              }


              return xml;
          }

    }
}