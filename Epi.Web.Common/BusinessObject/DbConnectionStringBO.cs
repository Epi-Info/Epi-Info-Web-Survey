using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.BusinessObject
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class DbConnectionStringBO
    {
        private int _DatasourceID;
        private string _DatasourceServerName;
        private string _DatabaseType;
        private string _InitialCatalog;
        private string _PersistSecurityInfo;
        private Guid _SurveyId;
        private string _DatabaseUserID;
        private string _Password;
        [DataMember]
        public int DatasourceID
        {
            get { return _DatasourceID; }
            set { _DatasourceID = value; }
        }

        [DataMember]
        public string DatasourceServerName
        {
            get { return _DatasourceServerName; }
            set { _DatasourceServerName = value; }
        }

        [DataMember]
        public string DatabaseType
        {
            get { return _DatabaseType; }
            set { _DatabaseType = value; }
        }

        [DataMember]
        public string InitialCatalog
        {
            get { return _InitialCatalog; }
            set { _InitialCatalog = value; }
        }

        [DataMember]
        public string PersistSecurityInfo
        {
            get { return _PersistSecurityInfo; }
            set { _PersistSecurityInfo = value; }
        }


        [DataMember]
        public Guid SurveyId
        {
            get { return _SurveyId; }
            set { _SurveyId = value; }
        }
        [DataMember]
        public string DatabaseUserID
        {
            get { return _DatabaseUserID; }
            set { _DatabaseUserID = value; }
        }
        [DataMember]
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
    }
}
