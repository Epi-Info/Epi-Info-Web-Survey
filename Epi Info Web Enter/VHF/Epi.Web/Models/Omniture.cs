using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
    {
    public class Omniture
        {
        private bool _IsEnabled;
        public bool IsEnabled
            {
            get { return _IsEnabled; }
            set { _IsEnabled = value; }
            }
       

        private string _ChannelName;
        public string ChannelName
            {
            get { return _ChannelName; }
            set { _ChannelName = value; }
            }
        private string _Level1;
        public string Level1
            {
            get { return _Level1; }
            set { _Level1 = value; }
            }

        private string _Level2;
        public string Level2
            {
            get { return _Level2; }
            set { _Level2 = value; }
            }
        private string _Level3;
        public string Level3
            {
            get { return _Level3; }
            set { _Level3 = value; }
            }

        private string _MetricUrl;
        public string MetricUrl
            {
            get { return _MetricUrl; }
            set { _MetricUrl = value; }
            }

       private string _TopicLevelJs;
        public string TopicLevelJs
            {
            get { return _TopicLevelJs; }
            set { _TopicLevelJs = value; }
            }

        private string _SCodeJs;
        public string SCodeJs
            {
            get { return _SCodeJs; }
            set { _SCodeJs = value; }
            }


        }

    }