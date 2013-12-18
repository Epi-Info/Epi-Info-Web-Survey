using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.DTO
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class CacheDependencyDTO 
    {
        private string _SurveyId;
        private DateTime _lastUpdate;

        [DataMember]
        public string SurveyId
        {
            get { return _SurveyId; }
            set { _SurveyId = value; }
        }

        [DataMember]
        public DateTime LastUpdate
        {
            get { return _lastUpdate; }
            set { _lastUpdate = value; }
        }
    }
}
