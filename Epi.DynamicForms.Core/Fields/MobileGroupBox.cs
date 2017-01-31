using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcDynamicForms.Fields
{
      [Serializable]
    public class MobileGroupBox : GroupBox
    {
        //public string Name { get; set; }
        //public bool Wrap { get; set; }
        //public string Html { get; set; }

        public override string RenderHtml()
        {
            //if (true)
            //{
                var wrapper = new TagBuilder(_fieldWrapper);

                if (string.IsNullOrEmpty(this._cssClass))
                {
                    wrapper.Attributes["class"] = _fieldWrapperClass;
                }

                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\r\n|\r|\n)+");

                string newText = regex.Replace(Html.Replace("  ", " &nbsp;"), "<br />");
                Html = MvcHtmlString.Create(newText).ToString();
                wrapper.Attributes["ID"] = "mvcdynamicfield_" + Name.ToLower() + "_groupbox_fieldWrapper";
                StringBuilder StyleValues = new StringBuilder();
                StyleValues.Append(GetMobileLiteralStyle(_fontstyle.ToString(), null, null, null, null, IsHidden));
                wrapper.Attributes.Add(new KeyValuePair<string, string>("style", StyleValues.ToString()));
                wrapper.InnerHtml = Html;
                return wrapper.ToString();
            //}

            return Html;
        }
        
        public string GetMobileLiteralStyle(string ControlFontStyle, string Top, string Left, string Width, string Height, bool IsHidden)
        {
            StringBuilder FontStyle = new StringBuilder();
            StringBuilder FontWeight = new StringBuilder();
            StringBuilder TextDecoration = new StringBuilder();
            StringBuilder CssStyles = new StringBuilder();

            char[] delimiterChars = { ' ', ',' };
            string[] Styles = ControlFontStyle.Split(delimiterChars);
            //CssStyles.Append("border-bottom: 2px solid #4e9689;color: #4e9689;font-size: 18px;font-weight: bold;line-height: 2em;");

            CssStyles.Append("border-bottom: 2px solid #4e9689;color: #4e9689;font-weight: bold;line-height: 2em;font:");

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
    }
}
