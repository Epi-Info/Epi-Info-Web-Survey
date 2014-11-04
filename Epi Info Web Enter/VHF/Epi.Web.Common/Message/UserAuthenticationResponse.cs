using System.Runtime.Serialization;
using Epi.Web.Enter.Common.MessageBase;
using Epi.Web.Enter.Common.DTO;
namespace Epi.Web.Enter.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class UserAuthenticationResponse : ResponseBase
    {
        public UserAuthenticationResponse() { }

        [DataMember]
        public bool UserIsValid;
        [DataMember]
        public string PassCode;

        [DataMember]
        public UserDTO User;
    }
}
