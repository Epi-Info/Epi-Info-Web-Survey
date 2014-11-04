using System;
using System.Web.Mvc;
using Epi.Web.MVC.Models;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using Epi.Core.EnterInterpreter;
using System.Collections.Generic;
using System.Web.Security;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using Epi.Web.EF;
using System.ServiceModel.Description;
using Epi.Web.Enter.Common.Security;
using System.Reflection;
using System.Diagnostics;
namespace Epi.Web.MVC.Controllers
{
    public class EIWSTController : Controller
    {
       //declare  SurveyFacade
        private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;
        private IEnumerable<XElement> PageFields;
        private  string RequiredList ="";
        private Epi.Web.Enter.Interfaces.DataInterfaces.IOrganizationDao OrganizationDao;
        /// <summary>
        /// injecting surveyFacade to the constructor 
        /// </summary>
        /// <param name="surveyFacade"></param>
        public EIWSTController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
        private enum TestResultEnum
            {
            Success,
            Error
            }
        //public ActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public ActionResult Index(string surveyid)
            {
            EIWSTModel TestModel = new EIWSTModel();
            try
                {

                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                ViewBag.Version = version;
               
               // string _connectionString = ConfigurationManager.AppSettings["TEST_CONNECTION_STRING"];
                //string connectionStringName = "EIWSADO";
                string connectionStringName = "EWEADO";
                //Decrypt connection string here
                string _connectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
          
               // string _connectionString ="Data Source=ETIDHAP56-SQL;Initial Catalog=OSELS_EIWS;User ID=SA;Password=put6uQ";
                using (var conn = new System.Data.SqlClient.SqlConnection(_connectionString))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    TestModel.DBTestStatus = TestResultEnum.Success.ToString();

                    cmd.CommandText = "SELECT * FROM  lk_Status";
                    cmd.Parameters.AddWithValue("@StatusId", 1);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }
                        var TestValue = reader.GetString(reader.GetOrdinal("Status"));
                     }
                      TestModel.DBTestStatus = TestResultEnum.Success.ToString();
                  }
                 
                
                }
            catch (Exception ex)
                {
              
                TestModel.DBTestStatus = TestResultEnum.Error.ToString();
                TestModel.STestStatus = "Incomplete";
                TestModel.EFTestStatus = "Incomplete";
                TempData["exc" ] = ex.Message.ToString();
                TempData["exc1"] = ex.Source.ToString();
                TempData["exc2"] = ex.StackTrace.ToString();
              

                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, TestModel);
                }


            try
                {
                Epi.Web.EF.EntityOrganizationDao NewEntity = new Epi.Web.EF.EntityOrganizationDao();
                List<Epi.Web.Enter.Common.BusinessObject.OrganizationBO> OrganizationBO = new List<Enter.Common.BusinessObject.OrganizationBO>();
                OrganizationBO = NewEntity.GetOrganizationNames();
                if (OrganizationBO != null)
                    {
                    TestModel.EFTestStatus = TestResultEnum.Success.ToString();
                    }

            
                }
            catch (Exception ex)
                {


                TestModel.EFTestStatus = TestResultEnum.Error.ToString();
                TestModel.STestStatus = "Incomplete";
                TempData["exc"] = ex.Message.ToString();
                TempData["exc1"] = ex.Source.ToString();
                TempData["exc2"] = ex.StackTrace.ToString();

                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, TestModel);
                }


            try
                {

             
             
                SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(surveyid);
                if (surveyInfoModel != null)
                    {
                    TestModel.STestStatus = TestResultEnum.Success.ToString();
                    }
                   return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, TestModel);
                 }
            catch (Exception ex)  
                {
                
               
                TestModel.STestStatus = TestResultEnum.Error.ToString();
                
                TempData["exc"] = ex.Message.ToString();
                TempData["exc1"] = ex.Source.ToString();
                TempData["exc2"] = ex.StackTrace.ToString();

                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, TestModel);
                }



          

            }

    }
     
}
