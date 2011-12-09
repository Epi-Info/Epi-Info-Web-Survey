using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents html to be rendered on the form.
    /// </summary>
    [Serializable]
    public class Literal : Field
    {
        /// <summary>
        /// Determines whether the rendered html will be wrapped by another element.
        /// </summary>
        public bool Wrap { get; set; }
        /// <summary>
        /// The html to be rendered on the form.
        /// </summary>
        public string Html { get; set; }

        public override string RenderHtml()
        {
            if (Wrap)
            
            {
                var wrapper = new TagBuilder(_fieldWrapper);
                if (string.IsNullOrEmpty(this._cssClass))
                {
                    wrapper.Attributes["class"] = _fieldWrapperClass;
                }
                else
                {
                    wrapper.Attributes["class"] = this._cssClass;
                }

               
                StringBuilder StyleValues = new StringBuilder();
              
                StyleValues.Append(GetContolStyle(_fontstyle.ToString()));

                wrapper.Attributes.Add(new KeyValuePair<string, string>("style", StyleValues.ToString()));  
                wrapper.InnerHtml = Html;
                return wrapper.ToString();
            }
            return Html;
        }
        private string GetContolStyle(string ControlFontStyle)
        {

            StringBuilder FontStyle  = new StringBuilder();
            StringBuilder  FontWeight  = new StringBuilder();
            StringBuilder  TextDecoration = new StringBuilder();
            StringBuilder CssStyles = new StringBuilder();

            char[] delimiterChars = { ' ', ',' };
            string[] Styles = ControlFontStyle.Split(delimiterChars);

            CssStyles.Append("position:absolute;left:" + this._left.ToString() +
                    "px;top:" + this._top.ToString() + "px" + ";width:" + Width.ToString() + "px" + ";Height:" + Height.ToString() + "px");

                  
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
         if (FontStyle.ToString() != "")
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

            if (TextDecoration.ToString() != "")
            {
                CssStyles.Append(";text-decoration:");
            }
            
            CssStyles.Append(TextDecoration);


            return CssStyles.ToString();

        }
    }
}
