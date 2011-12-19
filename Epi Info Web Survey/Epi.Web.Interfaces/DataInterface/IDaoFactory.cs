using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Interfaces.DataInterfaces
{
    /// <summary>
    /// Abstract factory interface. Creates data access objects.
    /// </summary>
    /// <remarks>
    /// GoF Design Pattern: Factory.
    /// </remarks>
    public interface IDaoFactory
    {

        /// <summary>
        /// Gets an order data access object.
        /// </summary>
        ISurveyInfoDao SurveyInfoDao { get; }


        ISurveyResponseDao SurveyResponseDao { get; }
    }

}
