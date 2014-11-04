using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
namespace Epi.Web.MVC.Mock
{
    class SurveyHelperTest
    {
        public static string GetXML()
        {
            XDocument xdoc = XDocument.Load("../../Mock/MetaDataXML.xml");

            return xdoc.ToString();
        }
    }
}
