namespace ODataQueryHelper.Core.Model
{
    /// <summary>
    /// Defines types of filter criteria branch
    /// </summary>
    public enum FilterCriteriaBranchType
	{
        /// <summary>
        /// Default or bracket
        /// </summary>
		Bracket,
        /// <summary>
        /// branch with AND condition
        /// </summary>
		And,
        /// <summary>
        /// branch with OR condition
        /// </summary>
		Or
	}
}
