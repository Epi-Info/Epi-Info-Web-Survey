using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html input field that will accept time (am/pm or 24 hrs) as a string  from the user.
    /// </summary>
    [Serializable]
    public abstract class TimePickerField : InputField
    {
        private string _regexMessage = "Invalid";
        private string _ControlValue;


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
        public string Value { get; set; }
        //public string Value { get { return _Value; } set { _Value = value; } }
        public override string Response
        {
            get { return Value; }
            set { Value = value; }

        }

        //Declaring the min value for decimal
        private string _lower;
        //Declaring the max value for decimal
        private string _upper;
        //Declaring the pattern field
        private string _pattern;

        public string Lower
        {
            get { return _lower; }
            set { _lower = value; }
        }

        public string Upper
        {
            get { return _upper; }
            set { _upper = value; }
        }

        public string Pattern
        {
            get { return _pattern; }
            set { _pattern = value; }
        }
        /// <summary>
        /// Server Side validation for Numeric TextBox  
        /// </summary>
        /// <returns></returns>
        public override bool Validate(string Dateformat)
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

            if (!string.IsNullOrEmpty(Response))
            {

                /*Description: 
                
                     matches 24hrs time or am/pm
                 */
                string regularExp = "^(\\d{1,2}):(\\d{2})(:(\\d{2}))?(\\s?(AM|am|PM|pm))?$" ;
                var regex = new Regex(regularExp);

                if (!regex.IsMatch(Value))
                {
                    //invalid: it is not a valid time matching the above regular expression
                    if (string.IsNullOrEmpty(Pattern))
                    {
                        Error = "Value must be a valid time (e.g. 09:24:45 AM|PM or in Military time 13:23:45)";
                    }
                    else
                    {
                        if (Pattern == "HH:MM:SS AMPM")
                        {
                            Error = "Value must be a valid time (e.g. 09:24:45 AM|PM )";
                        }
                        else
                        {
                            Error = "Value must be a valid Military time (e.g. 13:23:45)";
                        }
                    }
                    
                    return false;
                }
            }

            ClearError();
            return true;
        }

    }
}
