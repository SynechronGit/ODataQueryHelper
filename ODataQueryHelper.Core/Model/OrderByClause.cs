using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ODataQueryHelper.Core.Model
{
    public class OrderByClause
    {
		public List<OrderByNode> OrderByFields { get; protected set; }

		const string fieldSeparator = @"\,";
		const string orderBySeparator = @"\S+";
		public OrderByClause()
		{
			OrderByFields = new List<OrderByNode>();
		}

		public void Parse(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException(value);
			}
			List<string> fields = Regex.Split(value, fieldSeparator)?.ToList();
			if (fields?.Any() == true)
			{
				int seq = 1;
				fields.ForEach(o =>
				{
					//TODO - Validate using proper Regex and get parts 
					var parts = Regex.Split(o, orderBySeparator);
			
					if (parts.Length <= 2)
					{
						string field = parts[0];
						string direction = (parts.Length == 2) ? parts[1] : "asc";
						OrderByFields.Add(new OrderByNode
						{
							Sequence = seq,
							FieldName = field,
							Direction = (string.Compare(direction, "asc", true) == 0) ? OrderDirectionType.Ascending : OrderDirectionType.Descending
						});
						seq++;
						//TODO - validate using property expression
					}
				});
			}
		}
    }
}
