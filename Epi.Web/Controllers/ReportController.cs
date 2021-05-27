using System;
using System.Web.Mvc;
using Epi.Web.MVC.Models;
using Epi.Web.Common.Message;
using System.Text;

namespace Epi.Web.MVC.Controllers
{
    public class ReportController: Controller
    {
        private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;
        public ReportController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }

        [HttpGet]
        public ActionResult Index(string reportid)
        {
            ReportModel Model = new ReportModel();

            try
            {


                //dba3faf6-4417-4f6d-920a-e3d6b680932a
                Epi.Web.Common.Helper.SqlHelper.GetReportXml(reportid);


                PublishReportRequest PublishReportRequest = new PublishReportRequest();
                PublishReportRequest.ReportInfo.ReportId = reportid;
                PublishReportRequest.IncludHTML = false;

                PublishReportResponse result = _isurveyFacade.GetSurveyReport(PublishReportRequest);
              
                
                    
                    Model.DateCreated = result.Reports[0].CreatedDate.ToString();
                    Model.Reportid = result.Reports[0].ReportId;
                     StringBuilder html = new StringBuilder();
                    foreach (var Gadget in result.Reports[0].Gadgets)
                    {
                    html.Append(Gadget.GadgetHtml);

                    }
                    Model.ReportHtml = html.ToString();

                    return View(Model); ;
               

                
            }
            catch (Exception ex)
            {

                return Json(false);
            }

            

        }
    }
}