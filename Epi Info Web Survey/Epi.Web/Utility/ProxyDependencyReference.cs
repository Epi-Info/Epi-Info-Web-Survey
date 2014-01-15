using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Web;
using System.Web.Caching;
using Epi.Web.Common.Message;
namespace Epi.Web
{
    public class ProxyDependencyReference
    {
        static ProxyDependencyReference _instance;
        static Timer _internalTimer;
        static int _period = 60000;
        static Dictionary<string, DependencyReferenceToken> _reference;

        ProxyDependencyReference() { }

        public static ProxyDependencyReference Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new ProxyDependencyReference();
                }

                return _instance;
            }
        }

        public static void Initialise()
        {
            _reference = new Dictionary<string, DependencyReferenceToken>();

            if (_internalTimer == null)
            {
                TimerCallback timerCallback = new TimerCallback(InternalTimerCallback);
                _internalTimer = new Timer(timerCallback);
                _internalTimer.Change(_period, _period);
            }
        }

        public static DependencyReferenceToken Get(string identifier)
        {
            DependencyReferenceToken value = null;

            if (_reference.TryGetValue(identifier, out value) == false)
            {
                value = new DependencyReferenceToken(DateTime.Now);
                _reference.Add(identifier, value);
            }
            
            return value;
        }

        static void InternalTimerCallback(object sender)
        {
            Epi.Web.WCF.SurveyService.IDataService dataService;
            dataService = new Epi.Web.WCF.SurveyService.DataService();
            CacheDependencyRequest request = new CacheDependencyRequest();
            request.Criteria.SurveyIdList = new List<string>(_reference.Keys);
            CacheDependencyResponse cacheDependencyResponse = (CacheDependencyResponse)dataService.GetCacheDependencyInfo(request);
            Dictionary<string, DateTime> response = cacheDependencyResponse.SurveyDependency;

            foreach(KeyValuePair<string, DateTime> kvp in response)
            {
                if (_reference.ContainsKey(kvp.Key))
                {
                    _reference[kvp.Key].ChangeIfGreater(kvp.Value);
                }
                else
                {
                    DependencyReferenceToken dependencyUpdateToken = new DependencyReferenceToken(kvp.Value);
                    _reference.Add(kvp.Key, dependencyUpdateToken);
                }
            }
        } 
    }

    public delegate void DependencyChangedEventHandler(object sender, EventArgs e);

    public class DependencyReferenceToken 
    {
        public event DependencyChangedEventHandler Changed;
        
        private DateTime _timeStamp;

        public DependencyReferenceToken(DateTime dateTime)
        {
            _timeStamp = dateTime;
        }

        public void ChangeIfGreater(DateTime dateTime)
        {
            if (dateTime > _timeStamp)
            {
                _timeStamp = dateTime;

                if (Changed != null)
                {
                    Changed(this, EventArgs.Empty);
                }
            }
        }
    }
}