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
                wrapper.Attributes["class"] = _fieldWrapperClass;
                wrapper.InnerHtml = Html;
                return wrapper.ToString();
            }
            return Html;
        }
    }
}
