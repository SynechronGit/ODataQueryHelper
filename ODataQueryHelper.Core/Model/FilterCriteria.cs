using System;
using System.Collections.Generic;
using System.Text;

namespace ODataQueryHelper.Core.Model
{
    public class FilterCriteriaNode
    {
		public FilterCriteriaNode(string propertyName, FilterCriteriaType criteriaType, object valueToCheck)
		{
			PropertyName = propertyName;
			CriteriaType = criteriaType;
			ValueToCheck = valueToCheck;
		}

		public string PropertyName { get; set; }

		public FilterCriteriaType CriteriaType { get; set; }

		public object ValueToCheck { get; set; }
    }

	public class FilterCriteriaBranch
	{
		public FilterCriteriaBranch()
		{
			Nodes = new List<FilterCriteriaNode>();
			Branches = new List<FilterCriteriaBranch>();
			BranchType = FilterCriteriaBranchType.Bracket;
		}
		public FilterCriteriaBranch(FilterCriteriaBranchType branchType): this()
		{
			BranchType = branchType;
		}

		public FilterCriteriaBranch AddNode(string propertyName, FilterCriteriaType criteriaType, object valueToCheck)
		{
			//if (0)
			//{
			//	throw new ArgumentNullException(nameof(node));
			//}

			Nodes.Add(new FilterCriteriaNode(propertyName, criteriaType, valueToCheck));

			return this;
		}

		public FilterCriteriaBranch AddNode(FilterCriteriaNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException(nameof(node));
			}

			Nodes.Add(node);

			return this;
		}

		public FilterCriteriaBranch AddBranch(FilterCriteriaBranch branch)
		{
			if (branch == null)
			{
				throw new ArgumentNullException(nameof(branch));
			}

			Branches.Add(branch);

			return branch;
		}

		public List<FilterCriteriaNode> Nodes { get; set; }

		public List<FilterCriteriaBranch> Branches { get; set; }

		public FilterCriteriaBranchType BranchType { get; set; }
	}

	public enum FilterCriteriaBranchType
	{
		Bracket,
		And,
		Or
	}
}
