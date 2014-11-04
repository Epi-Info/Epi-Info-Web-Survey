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
        protected bool _IsHidden;
        protected bool _IsHighlighted;
        protected bool _IsDisabled;
        protected bool _IsPlaceHolder;
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
        public bool IsHidden { get { return this._IsHidden; } set { this._IsHidden = value; } }
        public bool IsHighlighted { get { return this._IsHighlighted; } set { this._IsHighlighted = value; } }
        public bool IsDisabled { get { return this._IsDisabled; } set { this._IsDisabled = value; } }

        public bool IsPlaceHolder  { get { return this._IsPlaceHolder; } set { this._IsPlaceHolder = value; } }
        
        /// <summary>
        /// This function generates control style 
        /// </summary>
        /// <param name="ControlFontStyle"></param>
        /// <returns></returns>
        public string GetContolStyle(string ControlFontStyle, string Top, string Left, string Width, string Height, bool IsHidden)
        {

            StringBuilder FontStyle = new StringBuilder();
            StringBuilder FontWeight = new StringBuilder();
            StringBuilder TextDecoration = new StringBuilder();
            StringBuilder CssStyles = new StringBuilder();

            char[] delimiterChars = { ' ', ',' };
            string[] Styles = ControlFontStyle.Split(delimiterChars);
            if (string.IsNullOrEmpty(Width))
            {
                CssStyles.Append("position:absolute;left:" + Left +
                    "px;top:" + Top + "px" + ";Height:" + Height + "px");

            }
            else
            {
                CssStyles.Append("position:absolute;left:" + Left +
                        "px;top:" + Top + "px" + ";width:" + Width + "px" + ";Height:" + Height + "px");
            }

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
            if (IsHidden)
            {
                CssStyles.Append(";display:none");
            }
            CssStyles.Append(TextDecoration);


            return CssStyles.ToString();

        }
        public string GetRadioListStyle(string ControlFontStyle, string Top, string Left, string Width, string Height, bool IsHidden)
        {

            StringBuilder FontStyle = new StringBuilder();
            StringBuilder FontWeight = new StringBuilder();
            StringBuilder TextDecoration = new StringBuilder();
            StringBuilder CssStyles = new StringBuilder();

            char[] delimiterChars = { ' ', ',' };
            string[] Styles = ControlFontStyle.Split(delimiterChars);
           
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
            if (IsHidden)
            {
                CssStyles.Append(";display:none");
            }
            CssStyles.Append(";display:inline");
            CssStyles.Append(TextDecoration);
            

            return CssStyles.ToString();

        }
        public virtual string GetXML() { return ""; }

    }
}
