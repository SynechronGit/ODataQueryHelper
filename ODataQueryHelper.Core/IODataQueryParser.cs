using ODataQueryHelper.Core.Model;

namespace ODataQueryHelper.Core
{
    /// <summary>
    /// OData query parser for Document
    /// </summary>
    /// <typeparam name="T">Document of type <typeparamref name="T"/></typeparam>
    public interface IODataQueryParser<T> where T : class
    {
        /// <summary>
        /// Try and Parse OData Query expression
        /// </summary>
        /// <param name="oDataExpression">OData standard query expression</param>
        /// <returns>Parser Document Query of type <typeparamref name="T"/></returns>
        DocumentQuery<T> TryParse(string oDataExpression);
    }
}