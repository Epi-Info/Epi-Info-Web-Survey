using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html input field that will accept a numeric response from the user.
    /// </summary>
    [Serializable]
    public abstract class NumericTextField : InputField
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
        public override bool Validate()
        {
            if (ReadOnly)
            {
                Required = false;
                ClearError();
                return true;
            }

            if (Response == null || (Response.IndexOf("_") != -1) || ((Response.IndexOf(".") != -1 && Response.Length == 1)))
            {
                Response = string.Empty;
            }

            if (string.IsNullOrEmpty(Response))
            {
                if (Required)
                {
                    Error = RequiredMessage;
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(Response))
            {

                /*Description: Accept only (0-9) integer and one decimal point(decimal point is also optional). 
                 After decimal point it accepts at least one numeric. 
                 This will be usefull in money related fields or decimal fields. (www.regexlib.com)  */
                /*** Matches  .568 | 8578 | 1234567.1234567 ****/
                /**** Non-Matches: 568. | 56.89.36 | 5.3.6.9.6 *****/
                // string regularExp = "^([0-9]*|\\d*\\.\\d{1}?\\d*)$";
                //string regularExp = "^-?([0-9]*|\\d*\\.\\d{1}?\\d*)$";
                // var regex = new Regex(regularExp);

                //if (!regex.IsMatch(Value))

                double testValue = 0.0; string Value = Response; string lower = Lower; string upper = Upper;
                CultureInfo us = new CultureInfo("en-US");
                string uiSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
                if (!double.TryParse(Response, NumberStyles.Any, CultureInfo.InvariantCulture, out testValue))
                {
                    //invalid: it is not numeric
                    Error = "Value must be a number";
                    return false;
                }
                else
                {

                    //invalid: not in between range
                    //first check if low and upper are not empty
                    if ((!string.IsNullOrEmpty(Lower)) && (!string.IsNullOrEmpty(Upper)))
                    {
                        //if the number is either less than the lower limit or greater than the upper limit raise error
                        /*if ((decimal.Parse(Response) < decimal.Parse(Lower)) || (decimal.Parse(Response) > decimal.Parse(Upper)))
                        {
                            Error = string.Format("Number must be in between {0} and {1}", Lower, Upper);
                            return false;
                        }*/
                                             
                        if (uiSep == ".")
                        {
                            if ((decimal.Parse(Response) < decimal.Parse(Lower)) || (decimal.Parse(Response) > decimal.Parse(Upper)))
                            {
                                Error = string.Format("Number must be in between {0} and {1}", Lower, Upper);
                                return false;
                            }
                        }
                        else
                        {
                            Value = Response.Replace(".", ",");
                            lower = Lower.Replace(".", ",");
                            upper = Upper.Replace(".", ",");
                            if ((decimal.Parse(Value) < decimal.Parse(lower)) || (decimal.Parse(Value) > decimal.Parse(upper)))
                            {
                                Error = string.Format("Number must be in between {0} and {1}", lower, upper);
                                return false;
                            }

                        }
                       

                    }
                    if (uiSep == ".")
                    {
                        //invalid: checking for lower limit
                        if ((!string.IsNullOrEmpty(Lower)) && (decimal.Parse(Response) < decimal.Parse(Lower)))
                        {
                            Error = string.Format("Number can not be less than {0}", Lower);
                            return false;
                        }
                        //invalid: checking the upper limit 
                        if ((!string.IsNullOrEmpty(Upper)) && (decimal.Parse(Response) > decimal.Parse(Upper)))
                        {
                            Error = string.Format("Number can not be greater than {0}", Upper);
                            return false;
                        }
                    }
                    else
                    {
                        Value = Response.Replace(".", ",");
                        lower = Lower.Replace(".", ",");
                        upper = Upper.Replace(".", ",");

                        if ((!string.IsNullOrEmpty(Lower)) && (decimal.Parse(Value) < decimal.Parse(lower)))
                        {
                            Error = string.Format("Number can not be less than {0}", lower);
                            return false;
                        }
                        //invalid: checking the upper limit 
                        if ((!string.IsNullOrEmpty(Upper)) && (decimal.Parse(Value) > decimal.Parse(upper)))
                        {
                            Error = string.Format("Number can not be greater than {0}", upper);
                            return false;
                        }

                    }


                }
            }

            ClearError();
            return true;

        }





    }
}
