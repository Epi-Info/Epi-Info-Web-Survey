using System.Runtime.Serialization;
using Epi.Web.Enter.Common.MessageBase;

namespace Epi.Web.Enter.Common.Message
{
    /// <summary>
    /// Represents a security token response message from web service to client.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class TokenResponse : ResponseBase
    {
        /// <summary>
        /// Default Constructor for TokenResponse.
        /// </summary>
        public TokenResponse() { }

        /// <summary>
        /// Overloaded Constructor for TokenResponse. Sets CorrelationId.
        /// </summary>
        /// <param name="correlationId"></param>
        public TokenResponse(string correlationId) : base(correlationId) { }

        /// <summary>
        /// Security token returned to the consumer
        /// </summary>
        [DataMember]
        public string AccessToken;
    }
}

