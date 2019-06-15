using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ODataQueryHelper.Core.Model
{
    /// <summary>
    /// Represents Order by clause provided in OData Query Expression
    /// </summary>
    public class OrderByClause<T> where T : class
    {
        const string fieldSeparator = @"\,";
        const string orderBySeparator = @"\s+";

        /// <summary>
        /// Collection of order by node 
        /// </summary>
		public List<OrderByNode<T>> OrderByNodes { get; protected set; }

        /// <summary>
        /// Creates new instance of OrderByClause
        /// </summary>
        public OrderByClause()
        {
            OrderByNodes = new List<OrderByNode<T>>();
        }

        /// <summary>
        /// Try and Parse Orderby expression from OData Query
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="expression"/> is not null or empty.</exception>
        /// <exception cref="Exception.PropertyNotFoundException">property name provided in field does not belong to <typeparamref name="T"/>></exception>
        /// <param name="expression">order by expression</param>
		public void TryParse(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException(expression);
            }
            List<string> fields = Regex.Split(expression, fieldSeparator)?.ToList();
            if (fields?.Any() == true)
            {
                int seq = 1;
                fields.ForEach(o =>
                {
                    var parts = Regex.Split(o, orderBySeparator);
                    if (parts.Length <= 2)
                    {
                        string field = parts[0];
                        string direction = (parts.Length == 2) ? parts[1] : "asc";
                        Type t = typeof(T);
                        var propInfo = t.GetProperty(field, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        if (propInfo == null)
                        {
                            Error.PropertyNotFound($"Property <{field}> not found for <{t.Name}> in $orderby.");
                        }
                        var newNode = new OrderByNode<T>
                        {
                            Sequence = seq,
                            PropertyName = propInfo.Name,
                            Direction = (string.Compare(direction, "asc", true) == 0) ? OrderByDirectionType.Ascending : OrderByDirectionType.Descending
                        };
                        CreateExpression(newNode);
                        OrderByNodes.Add(newNode);
                        seq++;
                    }
                });
            }
        }

        /// <summary>
        /// Creates Expression for OrderBy/OrderByDescending
        /// </summary>
        /// <param name="node">Order by node to create expression</param>
        private void CreateExpression(OrderByNode<T> node)
        {
            if (!string.IsNullOrWhiteSpace(node?.PropertyName))
            {
                var param = Expression.Parameter(typeof(T), "p");
                var parts = node.PropertyName.Split('.');
                Expression parent = param;
                foreach (var part in parts)
                {
                    parent = Expression.Property(parent, part);
                }

                if (parent.Type.IsValueType)
                {
                    var converted = Expression.Convert(parent, typeof(object));
                    node.Expression = Expression.Lambda<Func<T, object>>(converted, param);
                }
                else
                {
                    node.Expression = Expression.Lambda<Func<T, object>>(parent, param);
                }
            }
        }
    }
}
