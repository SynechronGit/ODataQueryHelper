namespace ODataQueryHelper.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Escapes OData special characeter based on StringToExpression library
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Escape(this string source)
        {
            source = source.Replace(@"\", @"\\");
            source = source.Replace(@"''", @"\'");

            return source;
        }
    }
}
