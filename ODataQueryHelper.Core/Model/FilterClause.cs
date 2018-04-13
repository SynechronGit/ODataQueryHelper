using System;

namespace ODataQueryHelper.Core.Model
{
    public class FilterClause
    {
		public FilterClause()
		{
			Root = new FilterCriteriaBranch();
		}
		public FilterCriteriaBranch Root { get; set; }

		public void TryParseFilter<T>(string expression)
		{
			if (string.IsNullOrEmpty(expression))
			{
				throw new ArgumentNullException(nameof(expression));
			}

            //TODO : Parsing Logic for Filter
            Type t = typeof(T);
			var tokens = expression.Split(" eq ");
			if (tokens.Length == 2)
			{
                var propInfo = t.GetProperty(tokens[0]);
				Root.AddNode(tokens[0], FilterCriteriaType.Equal, ConvertTo(tokens[1], propInfo.PropertyType));
			}
		}

        private object ConvertTo(string value, Type t)
        {
            return Convert.ChangeType(value, t);
            //switch(t.Name.ToLower())
            //{
            //    case "Int32":
            //        return 
            //}
        }
    }
}
