using System.Runtime.Serialization;
using Epi.Web.WCF.SurveyService.MessageBase;

namespace Epi.Web.WCF.SurveyService.Messages
{
    /// <summary>
    /// Respresents a logout request message from client to web service.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class LogoutRequest : RequestBase
    {
        // This derived class intentionally left blank
        // Base class has the required parameters.
    }
}
