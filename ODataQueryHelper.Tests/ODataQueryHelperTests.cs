using MongoDB.Bson;
using MongoDB.Driver;
using ODataQueryHelper.Core;
using ODataQueryHelper.MongoDB;
using Xunit;

namespace ODataQueryHelper.Tests
{
    public class Employee
    {
        public ObjectId _id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
    public class MongoDBQueryRunnerTests
    {
        [Fact]
        public void FilterTest()
        {
            var expression = "$filter=Age eq 10";
            var docQuery = new ODataQueryParser();
            var query =  docQuery.TryParse<Employee>(expression);

            Assert.NotEmpty(query.Filter.Root.Nodes);
        }


        [Fact]
        public void QueryRunnerTest()
        {
            var expression = "$filter=Age eq 10&$skip=0&$top=3";
            //var expression = "$orderby=Age asc,FirstName asc";
            var docQuery = new ODataQueryParser();
            var query = docQuery.TryParse<Employee>(expression);

            MongoClient client = new MongoClient("mongodb://localhost:27017");
            var collection = client.GetDatabase("mydb").GetCollection<BsonDocument>("employee");

            var runner = new MongoDBQueryRunner<Employee>();

            runner.Create(query);
            var list = runner.Query(collection);

            Assert.True(list.Count > 0);
        }
    }
}
