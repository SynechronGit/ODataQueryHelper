using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ODataQueryHelper.Core.Model
{
    public class SelectCriteria
    {
		protected List<string> fields;
		const string parsingRegEx = @"\,";
		SelectCriteria()
		{
			fields = new List<string>();
		}

		public void Parse(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException(nameof(value));
			}
			fields.AddRange(Regex.Split(value, parsingRegEx));
			//TODO : Validate against <T> and add valid ones and ignore/report back invalid.
		}
    }
}
