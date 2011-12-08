using System.Runtime.Serialization;
using Epi.Web.WCF.SurveyService.MessageBase;

namespace Epi.Web.WCF.SurveyService.Messages
{
    /// <summary>
    /// Respresents a security token request message from client to web service.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class TokenRequest : RequestBase
    {
        // Nothing needed here...
    }
}

