using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;
//using Epi.Web.DataServiceClient;
namespace Epi.Web.MVC.Repositories.Core
{
    /// <summary>
    /// TODO: For now commented. Later we will delete this class
    /// Base class for all Repositories. 
    /// Provides common repository functionality, including:
    ///   - Management of Service client.
    ///   - Offers common request-response correlation check.
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// Lazy loads DataServiceClient and stores it in Session object.
        /// </summary>
        //protected SurveyManagerClient Client
        //{
        //    get
        //    {
        //        // Check if not initialized yet
        //        if (HttpContext.Current.Session["DataServiceClient"] == null)
        //            HttpContext.Current.Session["DataServiceClient"] = new SurveyManagerClient("WSHttpBinding_ISurveyManager");

        //        // If current client is 'faulted' (due to some error), create a new instance.
        //        var client = HttpContext.Current.Session["DataServiceClient"] as SurveyManagerClient;
        //        if (client.State == CommunicationState.Faulted)
        //        {
        //            try { client.Abort(); }
        //            catch { /* no action */ }

        //            client = new SurveyManagerClient("WSHttpBinding_ISurveyManager");
        //            HttpContext.Current.Session["DataServiceClient"] = client;
        //        }
        //        return client;
        //    }
        //}

       
        /// <summary>
        /// Correlates requestid with returned response correlationId. 
        /// These must always match. If not, request and responses are not related.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        //protected void Correlate(RequestBase request, ResponseBase response)
        //{
        //    if (request.RequestId != response.CorrelationId)
        //        throw new ApplicationException("RequestId and CorrelationId do not match.");
        //}
    }
}