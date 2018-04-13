using System;

namespace ODataQueryHelper.Core.Model
{
    /// <summary>
    /// Represents Document Query which can be used to populate result over IQueryable resultset (e.g. MongoDB, SQL etc)
    /// </summary>
    public class DocumentQuery
    {
        /// <summary>
        /// Provides options to add filter for result
        /// </summary>
        public FilterClause Filter { get; protected set; }

        /// <summary>
        /// Provides options to sort result
        /// </summary>
        public OrderByClause OrderBy { get; protected set; }

        /// <summary>
        /// Skip number of result item 
        /// </summary>
        public int Skip { get; protected set; }

        /// <summary>
        /// Limit result set to number of item
        /// </summary>
        public int Top { get; protected set; }

        /// <summary>
        /// Provides raw query expressions
        /// </summary>
        public string RawQuery { get; protected set; }

        /// <summary>
        /// Creates new instance of DocumentQuery Parser
        /// </summary>
        public DocumentQuery()
        {
            Filter = new FilterClause();
            OrderBy = new OrderByClause();
        }

        /// <summary>
        /// Try and parse value provided in $skip of expression
        /// </summary>
        /// <param name="skip">$skip expression</param>
        public void TryParseSkip(string skip)
        {
            if (string.IsNullOrEmpty(skip))
            {
                Skip = 0;
                return;
            }
            if (!int.TryParse(skip, out int val))
            {
                Error.InvalidSkipValueException("Value of $skip can only be non-negative integer");
            }

            Skip = val;
        }

        /// <summary>
        /// Try and parse value provided in $top of expression
        /// </summary>
        /// <param name="top"></param>
        public void TryParseTop(string top)
        {
            if (!int.TryParse(top, out int val))
            {
                Error.InvalidTopValueException("Value of $top can only be non-negative integer.");
            }

            Top = val;
        }
    }
}
