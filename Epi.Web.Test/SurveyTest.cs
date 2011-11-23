using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Web;

using NUnit.Framework;
namespace Epi.Web.Test
{
    [TestFixture]
    public class SurveyTest
    {

        [Test]

        public void The_Controller_Renders_View()
        {
            Epi.Web.Controllers.HomeController controller = new Epi.Web.Controllers.HomeController();
            ActionResult result = controller.Index();
            Assert.AreEqual("System.Web.Mvc.ViewResult", result.ToString());
            
        }

        [Test]
        public void The_Controller_Gets_The_View()
        {
            Epi.Web.Controllers.HomeController controller = new Epi.Web.Controllers.HomeController();

        }


        public class SurveyManager:ISurveyManager
        {


            public SurveyRequestResult GetSurveyByGuid(string guid)
            {
                //throw new NotImplementedException();

                SurveyRequestResult sresult = new SurveyRequestResult();
                sresult.IsPulished = true;
                sresult.StatusText = "Succeed";
                sresult.URL = "/Survey/Result/id";
                return sresult;

            }
        }
        public interface ISurveyManager
        {
            public SurveyRequestResult GetSurveyByGuid(String guid=string.Empty);
        }

         public class SurveyRequestResult
        {
            bool isPulished;
            string uRL;
            string statusText;

            public bool IsPulished { get { return this.isPulished; } set { this.isPulished = value; } }

            public string URL { get { return this.uRL; } set { this.uRL = value; } }

            public string StatusText { get { return this.statusText; } set { this.statusText = value; } }
        }
    }
}
