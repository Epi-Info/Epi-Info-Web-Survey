using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Interfaces.DataInterfaces
{
    public interface IDaoFactory
    {
        /// <summary>
        /// Gets an order data access object.
        /// </summary>
        ISurveyInfoDao SurveyInfoDao { get; }
        ICacheDependencyInfoDao CacheDependencyInfoDao { get; }
        ISurveyResponseDao SurveyResponseDao { get; }
        IOrganizationDao OrganizationDao { get; }
    }
}
