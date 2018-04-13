namespace ODataQueryHelper.Core.Model
{ 
    /// <summary>
    /// Represents single order by field node
    /// </summary>
    public class OrderByNode
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
    }
}
