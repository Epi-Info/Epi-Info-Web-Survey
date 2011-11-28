﻿namespace MvcDynamicForms
{
    /// <summary>
    /// Class that represents the end user's response to an InputField object.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// The title of the InputField object.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The user's response.
        /// </summary>
        public string Value { get; set; }
    }
}
