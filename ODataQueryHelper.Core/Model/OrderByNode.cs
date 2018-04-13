namespace ODataQueryHelper.Core.Model
{ 
    public class OrderByNode
    {
		public int Sequence { get; set; }
		public string FieldName { get; set; }
		public OrderDirectionType Direction { get; set; }
    }
}
