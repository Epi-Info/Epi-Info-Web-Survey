using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
 
namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html input field that will accept a text response from the user.
    /// </summary>
    [Serializable]
    public abstract class TextField : InputField
    {
        private string _regexMessage = "Invalid";
        /// <summary>
        /// A regular expression that will be applied to the user's text respone for validation.
        /// </summary>
        public string RegularExpression { get; set; }        
        /// <summary>
        /// The error message that is displayed to the user when their response does no match the regular expression.
        /// </summary>
        public string RegexMessage
        {
            get
            {
                return _regexMessage;
            }
            set
            {
                _regexMessage = value;
            }
        }

        public override bool Validate()
        {
            /*If readonly don't perform any validation check and make required = false and validate = true*/
            if (ReadOnly)
            {
                Required = false;
                ClearError();
                return true;
            }

            if (string.IsNullOrEmpty(Response))
            {
                if (Required)
                {
                    // invalid: is required and no response has been given
                    Error = RequiredMessage;
                    return false;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(RegularExpression))
                {
                    var regex = new Regex(RegularExpression);
                    if (!regex.IsMatch(Response))
                    {
                        Error = RegexMessage;
                        return false;
                    }
                }
            }

            ClearError();
            return true;
        }
        public override string GetXML()
        {
            string XML ="<ResponseDetail QuestionId='"+_key + "'>" + Response +"<ResponseDetail>";
            return XML;
        }
    }
}
