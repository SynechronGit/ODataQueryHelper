using ODataQueryHelper.Core.Model;
using System;
using System.Web;

namespace ODataQueryHelper.Core
{
    /// <summary>
    /// OData query parser for Document
    /// </summary>
    /// <typeparam name="T">Document of type <typeparamref name="T"/></typeparam>
    public class ODataQueryParser<T> : IODataQueryParser<T> where T: class
    {
        /// <summary>
        /// Try and Parse OData Query expression
        /// </summary>
        /// <param name="oDataExpression">OData standard query expression</param>
        /// <returns>Parser Document Query of type <typeparamref name="T"/></returns>
        public DocumentQuery<T> TryParse(string oDataExpression)
        {
            var model = new DocumentQuery<T>();
            if (string.IsNullOrEmpty(oDataExpression))
            {
                throw new ArgumentNullException(nameof(oDataExpression));
            }

            var queryStrings = HttpUtility.ParseQueryString(oDataExpression);
            if (queryStrings.HasKeys())
            {
                var filterQuery = queryStrings["$filter"];
                if (!string.IsNullOrEmpty(filterQuery))
                {
                    model.Filter.TryParseFilter(filterQuery);
                }

                var orderByQuery = queryStrings["$orderby"];
                if (!string.IsNullOrEmpty(orderByQuery))
                {
                    model.OrderBy.TryParse(orderByQuery);
                }

                var skip = queryStrings["$skip"];
                if (!string.IsNullOrEmpty(skip))
                {
                    model.TryParseSkip(skip);
                }

                var top = queryStrings["$top"];
                if (!string.IsNullOrEmpty(top))
                {
                    model.TryParseTop(top);
                }
            }

            return model;

        }
    }
}
