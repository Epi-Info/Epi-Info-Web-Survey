using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web
{
    public class SurveyResponseXML
    {
        Dictionary<string,string> ResponseDetailList;


        public SurveyResponseXML()
        {

        }

        public void Add(MvcDynamicForms.Form pForm)
        {

            foreach (var field in pForm.InputFields)
            {
                if(this.ResponseDetailList.ContainsKey(field.Key))
                {
                    this.ResponseDetailList[field.Key] = field.GetXML();
                }
                else
                {
                    this.ResponseDetailList.Add(field.Key, field.GetXML());
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
    }
}