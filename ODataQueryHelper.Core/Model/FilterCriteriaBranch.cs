using System;
using System.Collections.Generic;

namespace ODataQueryHelper.Core.Model
{
    /// <summary>
    /// Represents Filter branch which may consists of more then one node as well as branch
    /// </summary>
    public class FilterCriteriaBranch
	{
        /// <summary>
        /// Filter criteria nodes
        /// </summary>
        public List<FilterCriteriaNode> Nodes { get; set; }
        /// <summary>
        /// Filter criteria branches
        /// </summary>
        public List<FilterCriteriaBranch> Branches { get; set; }

        /// <summary>
        /// Type of branch
        /// </summary>
        public FilterCriteriaBranchType BranchType { get; set; }

        /// <summary>
        /// Creates new instance of Criteria branch 
        /// </summary>
		public FilterCriteriaBranch()
		{
			Nodes = new List<FilterCriteriaNode>();
			Branches = new List<FilterCriteriaBranch>();
			BranchType = FilterCriteriaBranchType.Bracket;
		}
        /// <summary>
        /// Creates new instance of criteria branch
        /// </summary>
        /// <param name="branchType">Branch Type</param>
		public FilterCriteriaBranch(FilterCriteriaBranchType branchType): this()
		{
			BranchType = branchType;
		}

        /// <summary>
        /// Creates new Filter node and adds it into Nodes
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="criteriaType">Criteria type</param>
        /// <param name="valueToCheck">Value to check</param>
        /// <returns></returns>
		public FilterCriteriaBranch AddNode(string propertyName, FilterCriteriaType criteriaType, object valueToCheck)
		{
			if (string.IsNullOrEmpty(propertyName))
            {
                Error.Null(propertyName);
            }

			Nodes.Add(new FilterCriteriaNode(propertyName, criteriaType, valueToCheck));

			return this;
		}

        /// <summary>
        /// Adds new filter criteria node into nodes
        /// </summary>
        /// <param name="node">Single Filter criteria</param>
        /// <returns>Current instance</returns>
		public FilterCriteriaBranch AddNode(FilterCriteriaNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException(nameof(node));
			}

			Nodes.Add(node);

			return this;
		}

        /// <summary>
        /// Adds new filter criteria branch into branches
        /// </summary>
        /// <param name="branch">Filter criteria branch</param>
        /// <returns>Returns added criteria branch</returns>
		public FilterCriteriaBranch AddBranch(FilterCriteriaBranch branch)
		{
			if (branch == null)
			{
				throw new ArgumentNullException(nameof(branch));
			}

			Branches.Add(branch);

			return branch;
		}

		
	}
}
