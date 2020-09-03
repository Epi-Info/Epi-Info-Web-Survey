using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Epi.Web.Common.DTO
{
    public class ReportInfoDTO
    {

        private string _ReportId;

        private DateTime _CreatedDate;
        private DateTime _EditedDate;
        private int _ReportVersion;
        private List<GadgetDTO> _Gadgets;
        private string _ReportURL;

        private string _SurveyId;
        private string _DataSource;
        private int _RecordCount;
        [DataMember]
        public int RecordCount
        {

            get { return _RecordCount; }
            set { _RecordCount = value; }
        }
        [DataMember]
        public string DataSource
        {

            get { return _DataSource; }
            set { _DataSource = value; }
        }
        [DataMember]
        public string SurveyId
        {

            get { return _SurveyId; }
            set { _SurveyId = value; }
        }

        [DataMember]
        public string ReportURL
        {

            get { return _ReportURL; }
            set { _ReportURL = value; }
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
        public int ReportVersion
        {

            get { return _ReportVersion; }
            set { _ReportVersion = value; }
        }

        [DataMember]
        public List<GadgetDTO> Gadgets
        {

            get { return _Gadgets; }
            set { _Gadgets = value; }

        }
    }
}