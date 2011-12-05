using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Epi.Web.ActionServiceClient;
namespace Epi.Web.Repositories.Core
{
    /// <summary>
    /// Base class for all Repositories. 
    /// Provides common repository functionality, including:
    ///   - Management of Service client.
    ///   - Offers common request-response correlation check.
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// Lazy loads ActionServiceClient and stores it in Session object.
        /// </summary>
        protected SurveyManagerClient Client
        {
            get
            {
                // Check if not initialized yet
                if (HttpContext.Current.Session["ActionServiceClient"] == null)
                    HttpContext.Current.Session["ActionServiceClient"] = new SurveyManagerClient("WSHttpBinding_ISurveyManager");

                // If current client is 'faulted' (due to some error), create a new instance.
                var client = HttpContext.Current.Session["ActionServiceClient"] as SurveyManagerClient;
                if (client.State == CommunicationState.Faulted)
                {
                    try { client.Abort(); }
                    catch { /* no action */ }

                    client = new SurveyManagerClient("WSHttpBinding_ISurveyManager");
                    HttpContext.Current.Session["ActionServiceClient"] = client;
                }
                return client;
            }
        }

       
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