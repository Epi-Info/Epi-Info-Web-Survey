using System.Runtime.Serialization;
using Epi.Web.Common.MessageBase;

namespace Epi.Web.Common.Message
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
