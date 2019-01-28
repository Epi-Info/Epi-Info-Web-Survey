using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.Common.Json
{
    [Serializable]
    public class ResponseDetail
    {
        public ResponseDetail()
        {
            ResponseQA = new Dictionary<string, string>();
            ChildResponseDetailList = new List<ResponseDetail>();
        }

        public string ResponseId { get; set; }
        public string FormId { get; set; }
        public string FormName { get; set; }
        public string ParentResponseId { get; set; }
        public string ParentFormId { get; set; }

        public Dictionary<string, string> ResponseQA { get; set; }

        public List<ResponseDetail> ChildResponseDetailList { get; set; }
    }
}