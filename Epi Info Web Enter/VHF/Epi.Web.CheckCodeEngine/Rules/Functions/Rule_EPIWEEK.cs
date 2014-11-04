using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    /// <summary>
    /// Class for the Rule_Abs reduction.
    /// </summary>
    public partial class Rule_EPIWEEK : EnterRule
    {
        private List<EnterRule> ParameterList = new List<EnterRule>();

        public Rule_EPIWEEK(Rule_Context pContext, NonterminalToken pToken)
            : base(pContext)
        {
            this.ParameterList = EnterRule.GetFunctionParameters(pContext, pToken);
        }

        /// <summary>
        /// Executes the reduction.
        /// </summary>
        /// <returns>Returns the absolute value of two numbers.</returns>
        public override object Execute()
        {
            // Program code and logic by David Nitschke
            // Updated by Erik Knudsen
            
            object result = null;

            DateTime dt = DateTime.Now;
            if (this.ParameterList[0] != null && this.ParameterList[0].Execute() != null && DateTime.TryParse(this.ParameterList[0].Execute().ToString(), out dt))
            {
                string strAnswer;
                DateTime dteStart;
                int intYear;
                string strYear;
                DateTime dteQDate;
                DateTime dteQAccept;
                DateTime dteWkStart;
                DateTime dteWkEnd;
                DateTime dteEndOfQYr;
                int intMmwrWk;
                int intMmwrNow;
                int intMmwrMax;
                int intEndOfYrDay;

                dteQDate = dt;

                // The following lines of code make sure that if a NULL (blank) date is passed into this function
                // from Epi Info, that we don't cause an error to appear in Epi Info. Instead, we return a null
                // value and exit the function.

                if (dt == null)
                {
                    return null;
                }

                dteQAccept = dteQDate;

                // get the year
                intYear = dteQAccept.Year;

                // convert the year to a string
                //strYear = CStr(lngYear)
                strYear = intYear.ToString();

                //dteEndOfQYr = CDate("12/31/" & strYear)
                dteEndOfQYr = new DateTime(intYear, 12, 31);

                //intEndOfYrDay = Weekday(dteEndOfQYr)
                intEndOfYrDay = Microsoft.VisualBasic.DateAndTime.Weekday(dteEndOfQYr);

                if (intEndOfYrDay < 4 /*Microsoft.VisualBasic.Constants.vbWednesday*/)
                {
                    if (Microsoft.VisualBasic.DateAndTime.DateDiff("d", dteQAccept, dteEndOfQYr) < intEndOfYrDay)
                    {
                        //CDate("01/01/" & CStr(lngYear + 1))
                        dteQAccept = new DateTime(intYear + 1, 1, 1);
                    }

                    //TimeSpan timeSpan = dteQAccept.Subtract(dteEndOfQYr);
                    //if(timeSpan.Days < intEndOfYrDay) 
                    //{
                    //    dteQAccept = new DateTime(1, 1, intYear + 1);
                    //}
                }

                dteStart = GetMMWRStart(dteQAccept);

                if (dteStart > dteQAccept)
                {
                    //CDate("01/01/" & CStr(lngYear - 1)))
                    dteStart = GetMMWRStart(new DateTime(intYear - 1, 1, 1));
                }
                intMmwrWk = 1 + (int)Microsoft.VisualBasic.DateAndTime.DateDiff("w", dteStart, dteQAccept);
                strAnswer = intMmwrWk.ToString();

                if (strAnswer.Length < 2)
                {
                    strAnswer = "0" + strAnswer;
                }
                return (object)strAnswer;
            }
            else
            {
                return result;
            }
        }

        private DateTime GetMMWRStart(DateTime dteDateIn)
        {
            //'   GetMMWRStart returns the date of the start of the MMWR year closest to Jan 01
            //'   of the year passed in.  It finds 01/01/yyyy first then moves forward or back
            //'   the correct number of days to be the start of the MMWR year.  MMWR Week #1 is 
            //'   always the first week of the year that has a minimum of 4 days in the new year.
            //'   If Jan. first falls on a Thurs, Fri, or Sat, the MMWRStart date returned could be
            //'   greater than the date passed in so this must be checked for by the calling Sub.

            //'   If Jan. first is a Mon, Tues, or Wed, the MMWRStart goes back to the last
            //'   Sunday in Dec of the previous year which is the start of MMWR Week 1 for the
            //'   current year.

            //'   If the first of January is a Thurs, Fri, or Sat, the MMWRStart goes forward to 
            //'   the first Sunday in Jan of the current year which is the start of
            //'   MMWR Week 1 for the current year.  For example, if the year passed
            //'   in was 01/02/1998, a Friday, the MMWRStart that is returned is 01/04/1998, a Sunday.  
            //'   Since 01/04/1998 > 01/02/1998, we must subract a year and pass Jan 1 of the new
            //'   year into this function as in GetMMWRStart("01/01/1997").
            //'   The MMWRStart date would then be returned as the date of the first
            //'   MMWR Week of the previous year.    

            DateTime dteYrBegin;
            DateTime dteResult;
            double dblDayOfWeek;

            //CDate("01/01/" & CStr(Year(dteDateIn)))
            dteYrBegin = new DateTime(dteDateIn.Year, 1, 1);

            dblDayOfWeek = Microsoft.VisualBasic.DateAndTime.Weekday(dteYrBegin);

            if (dblDayOfWeek <= 4 /*vbWednesday*/)
            {
                dteResult = Microsoft.VisualBasic.DateAndTime.DateAdd("d", -(dblDayOfWeek - 1), dteYrBegin);
            }
            else
            {
                dteResult = Microsoft.VisualBasic.DateAndTime.DateAdd("d", ((7 - dblDayOfWeek) + 1), dteYrBegin);
            }

            return dteResult;
        }
    }
}
