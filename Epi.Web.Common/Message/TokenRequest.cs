using System.Runtime.Serialization;
using Epi.Web.Common.MessageBase;

namespace Epi.Web.Common.Message
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

