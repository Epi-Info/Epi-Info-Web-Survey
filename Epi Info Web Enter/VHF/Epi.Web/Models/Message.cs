using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.Models
{
   
    public interface IMessageService { string GetMessage();    }
    public class MessageService : IMessageService 
    { 
        public string GetMessage() 
        { 
            return "Hello from the MessageService"; 
        } 
    }
}