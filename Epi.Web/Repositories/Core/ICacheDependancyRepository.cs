using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.Message;

namespace Epi.Web.MVC.Repositories.Core
{
    public interface ICacheDependencyRepository: IRepository<Epi.Web.Common.Message.CacheDependencyResponse>
    {
        CacheDependencyResponse GetCacheDependencyInfo(CacheDependencyRequest request);
    }
}
