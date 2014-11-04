using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Core.EnterInterpreter
{
        /// <summary>
    /// We need to find a way to organize these methods to reduce the possibility
    /// of "Spaghetti Code" like maintenance problems. Please do not add any more members
    /// to this class. Thanks.
    /// </summary>
    public static class Util
    {

        private static DateTime nullDateTime = new DateTime();

        /// <summary>
        /// Gets a null value DateTime object.
        /// </summary>
        public static DateTime NullDateTime
        {
            get { return nullDateTime; }
        }

        /// <summary>
        /// Determines if the string object is empty.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Determines if the datetime object is empty.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsEmpty(DateTime dt)
        {
            if (((object)dt) == DBNull.Value) return true;
            else if (DateTime.Equals(dt, nullDateTime)) return true;
            else return false;
        }

        /// <summary>
        /// Tests an object for empty or null.
        /// </summary>
        /// <param name="obj">Any object.</param>
        /// <returns>Results of empty or null.</returns>
        public static bool IsEmpty(object obj)
        {
            if (obj == null) return true;
            else if (obj == DBNull.Value) return true;
            else if (string.IsNullOrEmpty(obj.ToString())) return true;
            else return false;
        }


    }
}
