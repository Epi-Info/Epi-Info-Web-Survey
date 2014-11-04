using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcDynamicForms.Fields
{ 
      [Serializable]
    public class MobileGroupBox : Field
    {

        /// <summary>
        /// Determines whether the rendered html will be wrapped by another element.
        /// </summary>
        /// 

        public string Name { get; set; }
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
                 //   wrapper.Attributes["class"] = this._cssClass;
                }


                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\r\n|\r|\n)+");

                string newText = regex.Replace(Html.Replace("  ", " &nbsp;"), "<br />");

                Html = MvcHtmlString.Create(newText).ToString();

                //wrapper.Attributes["ID"] = "labelmvcdynamicfield_" + Name.ToLower();
                wrapper.Attributes["ID"] = "mvcdynamicfield_" + Name.ToLower() + "_groupbox_fieldWrapper";

                StringBuilder StyleValues = new StringBuilder();

                StyleValues.Append(GetMobileLiteralStyle(_fontstyle.ToString(), null, null, null, null, IsHidden));
                //StyleValues.Append(";word-wrap:break-word;");

                // wrapper.Attributes.Add("data-role", "fieldcontain");
                wrapper.Attributes.Add(new KeyValuePair<string, string>("style", StyleValues.ToString()));

                wrapper.InnerHtml = Html;
                return wrapper.ToString();
            }
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
            //if (string.IsNullOrEmpty(Width))
            //{
            //    CssStyles.Append("position:absolute;left:" + Left +
            //        "px;top:" + Top + "px" + ";Height:" + Height + "px");

            //}
            //else
            //{
            //    CssStyles.Append("position:absolute;left:" + Left +
            //            "px;top:" + Top + "px" + ";width:" + Width + "px" + ";Height:" + Height + "px");
            //}
            CssStyles.Append("border-bottom: 2px solid #4e9689;color: #4e9689;font-size: 18px;font-weight: bold;line-height: 2em;");
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
            //CssStyles.Append(";font:");//1
            //if (!string.IsNullOrEmpty(FontStyle.ToString()))
            //{

            //    CssStyles.Append(FontStyle);//2
            //    CssStyles.Append(" ");//3
            //}
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
