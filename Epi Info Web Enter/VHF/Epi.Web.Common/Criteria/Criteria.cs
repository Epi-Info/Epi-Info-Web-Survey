using System.Runtime.Serialization;

namespace Epi.Web.Enter.Common.Criteria
{
    /// <summary>
    /// Base class that holds criteria for queries.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class Criteria
    {
        /// <summary>
        /// Sort expression of the criteria.
        /// </summary>
        [DataMember]
        public string SortExpression { get; set; }
    }
}