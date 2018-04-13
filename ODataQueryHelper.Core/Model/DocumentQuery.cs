namespace ODataQueryHelper.Core.Model
{
    public class DocumentQuery
    {
        public FilterClause Filter { get; protected set; }

        public OrderByClause OrderBy { get; protected set; }

        public string RawQuery { get; protected set; }

        public DocumentQuery()
        {
            Filter = new FilterClause();
            OrderBy = new OrderByClause();
        }
    }
}
