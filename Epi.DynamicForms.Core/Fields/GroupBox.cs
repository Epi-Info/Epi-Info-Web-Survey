using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Web.Common;
using Epi.Web.Common.Constants;

namespace MvcDynamicForms.Fields
{
    [Serializable]
    public class GroupBox : TextField
    {
        public bool Wrap { get; set; }
        public string Html { get; set; }

        public override string RenderHtml()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            var inputName = Constant.FIELDPREFIX + _key;
            string ErrorStyle = string.Empty;

            TagBuilder legendTag = new TagBuilder("legend");

            string prompt = Prompt.Replace("\r\n", "<br/>");
            legendTag.InnerHtml = "<span>" + prompt + "</span>";
            legendTag.Attributes.Add("class", "optionPrompt");
            StringBuilder styleValues = new StringBuilder();
            styleValues.Append(GetStyle(_fontstyle.ToString()));
            
            double legendWidth = _ControlWidth - 12;
            double legendLeft = 16;
            double legendTop = 0 -_fontSize;

            legendTag.Attributes.Add("style", 
                "position:absolute;" +
                "left:" + legendLeft.ToString() + "px;" + 
                "top:" + legendTop.ToString() + "px;" + 
                "width:" + legendWidth.ToString() + "px;" +
                styleValues.ToString());

            TagBuilder fieldsetTag = new TagBuilder("fieldset");
            fieldsetTag.InnerHtml = legendTag.ToString();
            fieldsetTag.Attributes.Add("name", inputName);
            fieldsetTag.Attributes.Add("id", inputName);
            fieldsetTag.Attributes.Add("type", "text");
            fieldsetTag.Attributes.Add("value", Response);
            
            fieldsetTag.Attributes.Add("style", 
                "position:absolute;" + 
                "left:" + _left.ToString() + "px;" + 
                "top:" + _top.ToString() + "px;" + 
                "width:" + _ControlWidth.ToString() + "px;" +
                "height:" + _ControlHeight.ToString() + "px;" +
                "background:" + _BackgroundColor
                );
            
            fieldsetTag.MergeAttributes(_inputHtmlAttributes);
            htmlBuilder.Append(fieldsetTag.ToString());

            TagBuilder wrapperTag = new TagBuilder(_fieldWrapper);
            wrapperTag.Attributes["class"] = _fieldWrapperClass;
            
            if (_IsHidden)
            {
                wrapperTag.Attributes["style"] = "display:none";
            }

            wrapperTag.Attributes["id"] = inputName + "_fieldWrapper";
            wrapperTag.InnerHtml = htmlBuilder.ToString();
            return wrapperTag.ToString();
        }

        public string GetStyle(string ControlFontStyle )
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

            CssStyles.Append("font:");
            
            if (!string.IsNullOrEmpty(FontStyle.ToString()))
            {
                CssStyles.Append(FontStyle);
                CssStyles.Append(" ");
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
    }
}
