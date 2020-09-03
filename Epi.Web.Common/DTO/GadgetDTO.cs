using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Epi.Web.Common.DTO
{
    public class GadgetDTO
    {
         
        private string _ReportId;
         
        private DateTime _CreatedDate;
        private DateTime _EditedDate;
        private int _GadgetVersion;
        private string _GadgetHtml;
        private string _GadgetURL;
        private Dictionary<string, string> _GadgetsScript = new   Dictionary<string, string>();
        private string _GadgetId;
        private int _GadgetNumber;
        [DataMember]
        public string GadgetId
        {

            get { return _GadgetId; }
            set { _GadgetId = value; }
        }
        [DataMember]
        public int GadgetNumber
        {

            get { return _GadgetNumber; }
            set { _GadgetNumber = value; }
        }
        [DataMember]
        public string GadgetURL
        {

            get { return _GadgetURL; }
            set { _GadgetURL = value; }
        }
       
     
        [DataMember]
       
        public string ReportId
        {

            get { return _ReportId; }
            set { _ReportId = value; }
        }

        [DataMember]
        public DateTime CreatedDate
        {

            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        
        [DataMember]
        public DateTime EditedDate
        {

            get { return _EditedDate; }
            set { _EditedDate = value; }
        }
        [DataMember]
        public int GadgetVersion
        {

            get { return _GadgetVersion; }
            set { _GadgetVersion = value; }
        }
        [DataMember]
        public Dictionary<string, string> GadgetsScript
        {

            get { return _GadgetsScript; }
            set { _GadgetsScript = value; }
        }
        [DataMember]
        public string GadgetHtml
        {

            get { return _GadgetHtml; }
            set { _GadgetHtml = value; }
        }
    }
}
