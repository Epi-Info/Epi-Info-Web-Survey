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

        protected double _top;
        protected double _left;
        protected string _cssClass;
        protected string _fontfamily;
        protected double _fontSize;
        protected string _fontstyle;


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


        public double Top { get { return this._top; } set { this._top = value; } }
        public double Left { get { return this._left; } set { this._left = value; } }
        public string CssClass { get { return this._cssClass; } set { this._cssClass = value; } }
        //_font
        public string fontfamily { get { return this._fontfamily; } set { this._fontfamily = value; } }
        public double fontSize { get { return this._fontSize; } set { this._fontSize = value; } }
        public string fontstyle { get { return this._fontstyle; } set { this._fontstyle = value; } }

         
        //
    }
}
