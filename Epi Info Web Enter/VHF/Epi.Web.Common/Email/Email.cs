using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Enter.Common.Email
{
    public class Email
    {

        private  List<string> _To;
        private string _From;
        private string _Body;
        private string _Subject;

        public   List<string> To 
        {
            get { return _To; }
            set { _To = value; }
        
        }
        public string From
        {
            get { return _From; }
            set { _From = value; }

        }
        public string Body
        {
            get { return _Body; }
            set { _Body = value; }

        }
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }

        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        
    }
}
