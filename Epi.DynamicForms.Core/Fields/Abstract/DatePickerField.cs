using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html input field that will accept a date as a string  from the user.
    /// </summary>
    [Serializable]
    public abstract class DatePickerField : InputField
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
                
                     Dates day: d or dd, &lt;= 31, month: m or mm, &lt;= 12, year: yy or yyyy &gt;= 1900, &lt;= 2099 
                     Matches: 01/01/2001 | 1/1/1999 | 10/20/2080 
                     Non-Matches: 13/01/2001 | 1/1/1800 | 10/32/2080 
                 */
                //mm/dd/yyyy
                //string regularExp = "^(((((((0?[13578])|(1[02]))[\\.\\-/]?((0?[1-9])|([12]\\d)|(3[01])))|(((0?[469])|(11))[\\.\\-/]?((0?[1-9])|([12]\\d)|(30)))|((0?2)[\\.\\-/]?((0?[1-9])|(1\\d)|(2[0-8]))))[\\.\\-/]?(((19)|(20))?([\\d][\\d]))))|((0?2)[\\.\\-/]?(29)[\\.\\-/]?(((19)|(20))?(([02468][048])|([13579][26])))))$";

                string regularExp = "^[0-9]{4}-(((0[13578]|(10|12))-(0[1-9]|[1-2][0-9]|3[0-1]))|(02-(0[1-9]|[1-2][0-9]))|((0[469]|11)-(0[1-9]|[1-2][0-9]|30)))$";

                var regex = new Regex(regularExp);

                if (!regex.IsMatch(Response))
                {
                    //invalid: it is not a valid date matching the above regular expression
                    Error = "Value must be a valid date";
                    return false;
                }
                else
                {
                    if (Lower == "SYSTEMDATE")
                    {

                        switch (Pattern.ToString())
                        {
                            case "YYYY-MM-DD":
                                Lower = DateTime.Now.ToString("YYYY - MM - DD");
                                break;
                            case "MM-DD-YYYY":
                                Lower = DateTime.Now.ToString("MM-DD-YYYY");
                                break;
                        }
                    } else if (Lower == "1/1/0001") {

                        Lower = "1/1/1901";
                    }
                    if (Upper == "SYSTEMDATE")
                    {

                        switch (Pattern.ToString())
                        {
                            case "YYYY-MM-DD":
                                Upper = DateTime.Now.ToString("yyyy-mm-dd");
                                break;
                            case "MM-DD-YYYY":
                                Upper = DateTime.Now.ToString("MM-DD-YYYY");
                                break;
                        }
                    }
                    else if (Upper == "1/1/0001")
                    {

                        Upper = "1/1/1901";
                    }
                    //invalid: not in between range
                    //first check if low and upper are not empty
                    if ((!string.IsNullOrEmpty(Lower)) && (!string.IsNullOrEmpty(Upper)))
                    {
                       
                        //if the date is either less than the lower limit or greater than the upper limit raise error
                        if ((DateTime.Parse(Response) < DateTime.Parse(Lower)) || (DateTime.Parse(Response) > DateTime.Parse(Upper)))
                        {
                            Error = string.Format("Date must be in between {0} and {1}", GetDateFormat(Lower, Pattern), GetDateFormat(Upper, Pattern));
                            return false;
                        }
                    }

                    //invalid: checking for lower limit
                    if ((!string.IsNullOrEmpty(Lower)) && (DateTime.Parse(Response) < DateTime.Parse(Lower)))
                    {
                        Error = string.Format("Date can not be less than {0}", GetDateFormat(Lower, Pattern));
                        return false;
                    }
                    //invalid: checking the upper limit 
                    if ((!string.IsNullOrEmpty(Upper)) && (DateTime.Parse(Response) > DateTime.Parse(Upper)))
                    {
                        Error = string.Format("Date can not be greater than {0}", GetDateFormat(Upper, Pattern));
                        return false;
                    }


                }
            }

            ClearError();
            return true;

        }

        public string GetDateFormat(string Date, string pattern)
        {
            StringBuilder NewDateFormat = new StringBuilder();
            if (Date == "SYSTEMDATE")
            {
                switch (pattern.ToString())
                {
                    case "YYYY-MM-DD":
                        Date = DateTime.Now.ToString("YYYY - MM - DD");
                        break;
                    case "MM-DD-YYYY":
                        Date = DateTime.Now.ToString("MM-DD-YYYY");
                        break;
                }
            }
                string MM = "";
            string DD = "";
            string YYYY = "";
            char splitChar = '/';
            if (!string.IsNullOrEmpty(Date))
            {
                if (Date.Contains('-'))
                {
                    splitChar = '-';
                }
                else
                {

                    splitChar = '/';
                }
                string[] dateList = Date.Split((char)splitChar);
                switch (pattern.ToString())
                {
                    case "YYYY-MM-DD":
                        MM = dateList[1];
                        DD = dateList[2];
                        YYYY = dateList[0];
                        break;
                    case "MM-DD-YYYY":
                        MM = dateList[0];
                        DD = dateList[1];
                        YYYY = dateList[2];
                        break;
                }

                NewDateFormat.Append(MM);
                NewDateFormat.Append('/');
                NewDateFormat.Append(DD);
                NewDateFormat.Append('/');
                NewDateFormat.Append(YYYY);
            }
            else
            {
                NewDateFormat.Append("");

            }
            return NewDateFormat.ToString();
        }



    }
}
