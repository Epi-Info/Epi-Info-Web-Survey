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
    public class GroupBox : TextField
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
            var html = new StringBuilder();
            var inputName = _form.FieldPrefix + _key;
            string ErrorStyle = string.Empty;
            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";


            if (_IsHidden)
            {
                IsHiddenStyle = "display:none";
            }
            if (_IsHighlighted)
            {
                IsHighlightedStyle = "background:yellow";
            }



            // prompt label
            var prompt = new TagBuilder("legend");
           
            prompt.SetInnerText(Prompt);
            //prompt.Attributes.Add("for", inputName);
            //prompt.Attributes.Add("class", "EpiLabel");

            StringBuilder StyleValues = new StringBuilder();
            StyleValues.Append(GetStyle(_fontstyle.ToString() ));

            double PromptSize = Prompt.Length * fontSize;

            if (PromptSize > this.ControlWidth )
            {
            prompt.Attributes.Add("style", StyleValues.ToString() + ";width:" + _ControlWidth.ToString() + "px" );
            }else{
              prompt.Attributes.Add("style", StyleValues.ToString()   );
            
            }
            //html.Append(prompt.ToString());


            var txt = new TagBuilder("fieldset");
            //var Subtxt = new TagBuilder("legend");
            //Subtxt.InnerHtml = Prompt;
            txt.InnerHtml= prompt.ToString();
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "text");
            // txt.Attributes.Add("value", Value);
            txt.Attributes.Add("value", Value);

            txt.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px" + ";height:" + _ControlHeight.ToString() + "px;");// + IsHiddenStyle);

            txt.MergeAttributes(_inputHtmlAttributes);
           // html.Append(txt.ToString(TagRenderMode.SelfClosing));
            html.Append(txt.ToString());

            


            var wrapper = new TagBuilder(_fieldWrapper);
            wrapper.Attributes["class"] = _fieldWrapperClass;
            if (_IsHidden)
            {
                wrapper.Attributes["style"] = "display:none";

            }
            wrapper.Attributes["id"] = inputName + "_fieldWrapper";
            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
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
            CssStyles.Append("font:");//1
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

    }
}
