using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        protected double _Width;
        protected double _Height;
        

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

        public double Width { get { return this._Width; } set { this._Width = value; } }
        public double Height { get { return this._Height; } set { this._Height = value; } }

        /// <summary>
        /// This function generates control style 
        /// </summary>
        /// <param name="ControlFontStyle"></param>
        /// <returns></returns>
        public string GetContolStyle(string ControlFontStyle, string Top, string Left, string Width, string Height)
        {

            StringBuilder FontStyle = new StringBuilder();
            StringBuilder FontWeight = new StringBuilder();
            StringBuilder TextDecoration = new StringBuilder();
            StringBuilder CssStyles = new StringBuilder();

            char[] delimiterChars = { ' ', ',' };
            string[] Styles = ControlFontStyle.Split(delimiterChars);

            CssStyles.Append("position:absolute;left:" + Left +
                    "px;top:" + Top + "px" + ";width:" + Width + "px" + ";Height:" + Height + "px");


            foreach (string Style in Styles)
            {
                switch (Style.ToString())
                {
                    case "Italic":
                        FontStyle.Append(Style.ToString());
                        break;
                    case "Oblique":
                        FontStyle.Append(Style.ToString());

                        break;

                }

            }
            foreach (string Style in Styles)
            {
                switch (Style.ToString())
                {
                    case "Bold":
                        FontWeight.Append(Style.ToString());
                        break;
                    case "Normal":
                        FontWeight.Append(Style.ToString());

                        break;

                }

            }
            CssStyles.Append(";font:");//1
            if (!string.IsNullOrEmpty(FontStyle.ToString()))
            {

                CssStyles.Append(FontStyle);//2
                CssStyles.Append(" ");//3
            }
            CssStyles.Append(FontWeight);
            CssStyles.Append(" ");
            CssStyles.Append(_fontSize.ToString() + "pt ");
            CssStyles.Append(" ");
            CssStyles.Append(_fontfamily.ToString());

            foreach (string Style in Styles)
            {
                switch (Style.ToString())
                {
                    case "Strikeout":
                        TextDecoration.Append("line-through");
                        break;
                    case "Underline":
                        TextDecoration.Append(Style.ToString());

                        break;

                }

            }

            if (!string.IsNullOrEmpty(TextDecoration.ToString()))
            {
                CssStyles.Append(";text-decoration:");
            }

            CssStyles.Append(TextDecoration);


            return CssStyles.ToString();

        }

        public virtual string GetXML() { return ""; }

    }
}
