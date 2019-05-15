using System.Runtime.Serialization;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.DTO;
using System.Collections.Generic;

namespace Epi.Web.Common.Message
    {
    public class UserResponse
        {

        [DataMember]
        public List<UserDTO> User;
         [DataMember]
        public string Message;
        }
    }
