using System.Runtime.Serialization;
using Epi.Web.WCF.SurveyService.MessageBase;

namespace Epi.Web.WCF.SurveyService.Messages
{
    /// <summary>
    /// Represents a logout response message from web service to client.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class LogoutResponse : ResponseBase
    {
        /// <summary>
        /// Default Constructor for LogoutResponse.
        /// </summary>
        public LogoutResponse() { }

        /// <summary>
        /// Overloaded Constructor for LogoutResponse. Sets CorrelationId.
        /// </summary>
        /// <param name="correlationId"></param>
        public LogoutResponse(string correlationId) : base(correlationId) { }
    }
}