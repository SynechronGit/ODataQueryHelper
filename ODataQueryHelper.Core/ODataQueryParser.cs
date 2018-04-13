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
            }

            return model;

        }
    }
}
