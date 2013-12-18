using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Caching;

namespace Epi.Web
{
    public class ProxyDependency : CacheDependency
    {
        public ProxyDependency(string identifier)
        {
            DependencyReferenceToken dependencyReferenceToken = ProxyDependencyReference.Get(identifier);
            dependencyReferenceToken.Changed += new DependencyChangedEventHandler(ProxyDependency_Changed);
        }

        void ProxyDependency_Changed(object sender, EventArgs e)
        {
            NotifyDependencyChanged(this, new EventArgs());
        }
    }
}