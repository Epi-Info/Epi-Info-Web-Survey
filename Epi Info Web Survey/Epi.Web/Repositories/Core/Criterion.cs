using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
namespace Epi.Web.MVC.Repositories.Core
{
    /// <summary>
    /// Criterion stores and transports query criteria inluding sort orders and filtering.
    /// </summary>
    public class Criterion
    {
        /// <summary>
        /// Default constructor for Criterion.
        /// </summary>
        public Criterion() { }

        /// <summary>
        /// Overloaded constructor for Criterion.
        /// </summary>
        /// <param name="sort">The sort column.</param>
        /// <param name="order">The sort order.</param>
        public Criterion(string sort, string order)
        {
            Sort = sort;
            Order = order;
        }

        /// <summary>
        /// Overloaded contstructor for Criterion.
        /// </summary>
        /// <param name="attribute">The filter attribute (column name).</param>
        /// <param name="operator">The operator (enumerable: Equals, GreaterThan, etc).</param>
        /// <param name="operand">The operand (filter value)</param>
        public Criterion(string attribute, Operator @operator, object operand)
        {
            Filters.Add(new Filter(attribute, @operator, operand));
        }

        /// <summary>
        /// Overloaded contstructor for Criterion.
        /// </summary>
        /// <param name="sort">The sort column.</param>
        /// <param name="order">The sort order.</param>
        /// <param name="attribute">The filter attribute (column name).</param>
        /// <param name="operator">The operator (enumerable: Equals, GreaterThan, etc).</param>
        /// <param name="operand">The operand (filter value)</param>
        public Criterion(string sort, string order, string attribute, Operator @operator, object operand)
        {
            Sort = sort;
            Order = order;

            AddFilter(attribute, @operator, operand);
        }

        /// <summary>
        /// Adds a filter to Criterion.
        /// </summary>
        /// <param name="attribute">The filter attribute (column name).</param>
        /// <param name="operator">The operator (enumerable: Equals, GreaterThan, etc).</param>
        /// <param name="operand">The operand (filter value)</param>
        public void AddFilter(string attribute, Operator @operator, object operand)
        {
            Filters.Add(new Filter(attribute, @operator, operand));
        }


        #region Sorting

        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// Returns the combined Sort and Order in a valid SQL OrderExpression.
        /// </summary>
        public string OrderByExpression
        {
            get
            {
                if (string.IsNullOrEmpty(Sort) || string.IsNullOrEmpty(Order))
                    return "";

                return Sort + " " + Order;
            }
        }
        #endregion

        #region Filter

        // This is where user sets filter selections

        /// <summary>
        /// List of filters.
        /// </summary>
        public List<Filter> Filters = new List<Filter>();

        // Private working parameter list
        private List<object> parms = new List<object>();

        /// <summary>
        /// List of parameters that is used in DAOs.
        /// </summary>
        public object[] Parms { get { return parms.ToArray(); } }

        /// <summary>
        /// Returns a SQL where clause based on filter entries.
        /// </summary>
        public string WhereExpression
        {
            get
            {
                parms.Clear();

                if (Filters.Count == 0) return "";

                var sb = new StringBuilder();

                foreach (Filter filter in Filters)
                {
                    sb.Append(BuildFilterExpression(filter));
                    sb.Append(" AND ");
                }

                // Remove last AND
                return sb.Remove(sb.Length - 5, 5).ToString();
            }
        }

        // Helper method. Builds SQL expression from a filter entry.
        private string BuildFilterExpression(Filter filter)
        {
            if (!string.IsNullOrEmpty(filter.Attribute))
            {
                string att = filter.Attribute;

                // This will strip off prefixed alias names (e.g. U.UserId -> UserId)
                // Parameter names cannot be prefixed with table names or table aliases
                string parm = (att.IndexOf('.') >= 0) ? att.Substring(att.IndexOf('.') + 1) : att;

                switch (filter.Operator)
                {
                    case Operator.Equals:
                        parms.Add(parm); parms.Add(filter.Operand);
                        if (filter.Operand == null) return att + " IS NULL ";
                        return att + " = @" + parm;
                    case Operator.NotEquals:
                        parms.Add(parm); parms.Add(filter.Operand);
                        if (filter.Operand == null) return att + " IS NOT NULL ";
                        return att + " <> @" + parm;
                    case Operator.Contains:
                        parms.Add(parm); parms.Add("%" + filter.Operand + "%");
                        return att + " LIKE @" + parm;
                    case Operator.StartsWith:
                        parms.Add(parm); parms.Add(filter.Operand + "%");
                        return att + " LIKE @" + parm;
                    case Operator.EndsWith:
                        parms.Add(parm); parms.Add("%" + filter.Operand);
                        return att + " LIKE @" + parm;
                    case Operator.GreaterThan:
                        parms.Add(parm); parms.Add(filter.Operand);
                        return att + " > @" + parm;
                    case Operator.LessThan:
                        parms.Add(parm); parms.Add(filter.Operand);
                        return att + " < @" + parm;
                    case Operator.GreaterThanOrEqual:
                        parms.Add(parm); parms.Add(filter.Operand);
                        return att + " >= @" + parm;
                    case Operator.LessThanOrEqual:
                        parms.Add(parm); parms.Add(filter.Operand);
                        return att + " <= @" + parm;
                    case Operator.In:
                        return att + " IN (" + filter.Operand + ") ";
                }
            }
            return "";
        }

        #endregion

    }

    /// <summary>
    /// Represents a filter entry: with attribute, operator, operand.
    /// </summary>
    public class Filter
    {
        public string Attribute { get; set; }
        public Operator Operator { get; set; }
        public object Operand { get; set; }

        /// <summary>
        /// Filter constructor.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="operator"></param>
        /// <param name="operand"></param>
        public Filter(string attribute, Operator @operator, object operand)
        {
            Attribute = attribute;
            Operator = @operator;
            Operand = operand;
        }
    }

    /// <summary>
    /// Operator enumerations.
    /// </summary>
    public enum Operator
    {
        Contains,
        StartsWith,
        EndsWith,
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        In
    }
}