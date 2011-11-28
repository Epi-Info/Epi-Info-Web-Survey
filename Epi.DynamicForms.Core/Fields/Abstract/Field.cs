using System.Web.Mvc;
using System;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents a dynamically generated form field.
    /// </summary>
    [Serializable]
    public abstract class Field
    {
        protected string _fieldWrapper = "div";
        protected Form _form;
        protected string _fieldWrapperClass = "MvcFieldWrapper";

        internal Form Form
        {
            get
            {
                return _form;
            }
            set
            {
                _form = value;
            }
        }
        /// <summary>
        /// The html element that will be used to wrap fields when they are rendered as html.
        /// </summary>
        public string FieldWrapper
        {
            get
            {
                return _fieldWrapper;
            }
            set
            {
                _fieldWrapper = value;
            }
        }
        /// <summary>
        /// The class attribute of the wrapping html element.
        /// </summary>
        public string FieldWrapperClass
        {
            get
            {
                return _fieldWrapperClass;
            }
            set
            {
                _fieldWrapperClass = value;
            }
        }     
        /// <summary>
        /// The relative position that the field is rendered to html.
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Renders the field as html.
        /// </summary>
        /// <returns>Returns a string containing the rendered html of the Field object.</returns>
        public abstract string RenderHtml();
    }
}
