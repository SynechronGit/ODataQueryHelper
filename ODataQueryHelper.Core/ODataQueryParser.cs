using ODataQueryHelper.Core.Model;
using System;
using System.Web;

namespace ODataQueryHelper.Core
{
    public class ODataQueryParser
    {
        public DocumentQuery TryParse<T>(string oDataExpression)
        {
            var model = new DocumentQuery();
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
                    model.Filter.TryParseFilter<T>(filterQuery);
                }

                var orderByQuery = queryStrings["$orderby"];
                if (!string.IsNullOrEmpty(orderByQuery))
                {
                    model.OrderBy.TryParse<T>(orderByQuery);
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
