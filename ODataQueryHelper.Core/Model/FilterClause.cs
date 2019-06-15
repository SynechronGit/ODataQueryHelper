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
        }
        
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
