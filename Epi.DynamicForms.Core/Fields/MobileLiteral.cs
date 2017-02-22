using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace MvcDynamicForms.Fields
{
    [Serializable]
    public class MobileLiteral : Literal
    {
        public override string GetControlStyle(string ControlFontStyle, string top, string left, string width, string height, bool isHidden)
        {
            StringBuilder FontStyle = new StringBuilder();
            StringBuilder FontWeight = new StringBuilder();
            StringBuilder TextDecoration = new StringBuilder();
            StringBuilder CssStyles = new StringBuilder();

          /*  char[] delimiterChars = { ' ', ',' };
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
            
            CssStyles.Append(TextDecoration);*/

         //   prompt.Attributes.Add("for", inputName);
           // CssStyles.Append("class= EpiLabel");
           // prompt.Attributes.Add("Id", "label" + inputName);

            CssStyles.Append(" width: auto");

            return CssStyles.ToString();
        }
    }
}
