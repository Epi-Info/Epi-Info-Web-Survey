using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Common.DTO
    {
  public  class SurveyControlDTO
        {
        private string _ControlId;
        private string _ControlPrompt;
        private string _ControlType;

        public string ControlId
            {
            get { return _ControlId; }
            set { _ControlId = value; }
            }


        public string ControlPrompt
            {
            get { return _ControlPrompt; }
            set { _ControlPrompt = value; }
            }


        public string ControlType
            {
            get { return _ControlType; }
            set { _ControlType = value; }
            }
        }
    }
