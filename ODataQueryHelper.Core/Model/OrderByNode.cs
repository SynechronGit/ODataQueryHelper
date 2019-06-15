using System;
using System.Linq.Expressions;

namespace ODataQueryHelper.Core.Model
{ 
    /// <summary>
    /// Represents single order by field node
    /// </summary>
    public class OrderByNode<T>
    {
        /// <summary>
        /// Field Sequence
        /// </summary>
		public int Sequence { get; set; }
        /// <summary>
        /// Property Name
        /// </summary>
		public string PropertyName { get; set; }
        /// <summary>
        /// Order by direction (Asc or Dsc)
        /// </summary>
		public OrderByDirectionType Direction { get; set; }

        /// <summary>
        /// Gets or sets Order by expression to use in LINQ
        /// </summary>
        public Expression<Func<T, object>> Expression { get; set; }

    }
}
