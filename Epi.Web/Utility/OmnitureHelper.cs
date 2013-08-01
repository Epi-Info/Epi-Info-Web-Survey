using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Models;
using System.Configuration;

namespace Epi.Web.MVC.Utility
    {
    public class OmnitureHelper
        {


        public static Omniture GetSettings(string SurveyMode, bool IsMobileDevice) 
            {
         
            Omniture OmnitureObj = new Omniture();
            string IsEnabled = ConfigurationManager.AppSettings["OMNITURE_IS_ENABLED"];
            if (IsEnabled.ToUpper() == "TRUE")
                {
                OmnitureObj.IsEnabled = true;
                OmnitureObj.Level1 = ConfigurationManager.AppSettings["OMNITURE_LEVEL_1"];
                OmnitureObj.Level2 = ConfigurationManager.AppSettings["OMNITURE_LEVEL_2"];
                OmnitureObj.Level3 = ConfigurationManager.AppSettings["OMNITURE_LEVEL_3"];
                OmnitureObj.TopicLevelJs = ConfigurationManager.AppSettings["OMNITURE_TOPIC_LEVEL_JS"];
                OmnitureObj.SCodeJs = ConfigurationManager.AppSettings["OMNITURE_S_CODE_JS"];
                OmnitureObj.MetricUrl = ConfigurationManager.AppSettings["OMNITURE_METRIC_URL"];
                OmnitureObj.ChannelName = ConfigurationManager.AppSettings["OMNITURE_CHANNEL_NAME"];
                //if (IsMobileDevice)
                //    {
                //      OmnitureObj.ChannelName = ConfigurationManager.AppSettings["OMNITURE_CHANNEL_NAME"] + "_" + SurveyMode.ToString().ToUpper()+"_MOBILE";
                //    }
                //else
                //    {
                //      OmnitureObj.ChannelName = ConfigurationManager.AppSettings["OMNITURE_CHANNEL_NAME"] + "_" + SurveyMode.ToString();
                    
                //    }
                }
            else 
                {
                  OmnitureObj.IsEnabled = false;
                }

          return OmnitureObj;
            
            
            }




        }
    }