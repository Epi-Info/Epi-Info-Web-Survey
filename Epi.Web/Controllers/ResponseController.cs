using System;
using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using System.Collections.Generic;
using System.Text;
namespace Epi.Web.Controllers
{
    public class ResponseController : Controller
    {



   

        [HttpGet]
        public ActionResult Index(string surveyId ,string responseid, int PageNumber = 1)
        {

             
            try
            {
                
                TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = responseid.ToString();

                return RedirectToRoute(new { Controller = "Survey", Action = "Index", surveyId = surveyId, PageNumber = PageNumber });
                
            }
            catch (Exception ex)
            {
                return RedirectToAction(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
       
        }


        

    }
}
