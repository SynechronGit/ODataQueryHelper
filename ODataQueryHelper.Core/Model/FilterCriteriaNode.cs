namespace ODataQueryHelper.Core.Model
{
    /// <summary>
    /// Represent single filter condition from filter expression
    /// </summary>
    public class FilterCriteriaNode
    {
        /// <summary>
        /// Creates new instance of FilterCriteriaNode
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <param name="criteriaType">Filter criteria type</param>
        /// <param name="valueToCheck">Value to check against criteria</param>
		public FilterCriteriaNode(string propertyName, FilterCriteriaType criteriaType, object valueToCheck)
		{
			PropertyName = propertyName;
			CriteriaType = criteriaType;
			ValueToCheck = valueToCheck;
		}

        /// <summary>
        /// Name of Property (left hand)
        /// </summary>
		public string PropertyName { get; set; }

        /// <summary>
        /// Criteria Type (Operator)
        /// </summary>
		public FilterCriteriaType CriteriaType { get; set; }

        /// <summary>
        /// Value to check
        /// </summary>
		public object ValueToCheck { get; set; }
    }
}
