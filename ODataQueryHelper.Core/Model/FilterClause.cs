using ODataQueryHelper.Core.Language;
using System;
using System.Linq.Expressions;

namespace ODataQueryHelper.Core.Model
{
    /// <summary>
    /// Represent Filter Clause provided in OData Query expression
    /// </summary>
    public class FilterClause<T> where T : class
    {
        /// <summary>
        /// Creates new instance of FilterClause
        /// </summary>
        public FilterClause()
        {
            Root = new FilterCriteriaBranch();
        }
        
        //public Expression<Func<T,bool>> FilterExpression { get; set; }

        /// <summary>
        /// Root criteria branch. Typically bracket 
        /// </summary>
        public FilterCriteriaBranch Root { get; set; }

        /// <summary>
        /// Parsed filter expression
        /// </summary>
        public Expression<Func<T, bool>> FilterExpression { get; private set; }

        /// <summary>
        /// Parse Filter Cluase and populate Filter criteria.
        /// </summary>
        /// <typeparam name="T">Entity for which filter criteria is being popuplated</typeparam>
        /// <param name="filterExpression">filter query expression</param>
        public void TryParseFilter(string filterExpression)
        {
            if (string.IsNullOrEmpty(filterExpression))
            {
                throw new ArgumentNullException(nameof(filterExpression));
            }

            var mongoDbFilterLang = new MongoDBFilterLanguage();

            FilterExpression = mongoDbFilterLang.Parse<T>(filterExpression);

            ////TODO : Parsing Logic for Filter
            //Type t = typeof(T);
            //var tokens = filterExpression.Split(" eq ");
            //if (tokens.Length == 2)
            //{
            //    var propInfo = t.GetProperty(tokens[0]);
            //    if (propInfo == null)
            //    {
            //        Error.PropertyNotFound($"Property <{tokens[0]}> not found for <{t.Name}> in $filter.");
            //    }
            //    Root.AddNode(tokens[0], FilterCriteriaType.Equal, ConvertTo(tokens[1], propInfo.PropertyType));
            //}
            //else
            //{
            //    throw new NotImplementedException("Filter function is not supported yet.");
            //}
        }


        /// <summary>
        /// Converts string value to desired typed value. Works with premitive types only.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="t">Type of Convert value into</param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Converted typed value</returns>
        private object ConvertTo(string value, Type t)
        {
            return Convert.ChangeType(value, t);
        }
    }
}
