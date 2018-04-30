using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using ODataQueryHelper.Core;
using ODataQueryHelper.MongoDB;
using StringToExpression.GrammerDefinitions;
using StringToExpression.LanguageDefinitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

    public class Company
    {
        public Object _id { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public int NumberOfEmployees { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public DateTime ClosureDate { get; set; }
        public string FederalTaxIdentificationNumber { get; set; }
        public bool IsActive { get; set; }
        public string CompanyGroup { get; set; }
        public string Notes { get; set; }
        public string CollabGroup { get; set; }
        public int CurrentGroupID { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }


    }
    public class MongoDBQueryRunnerTests
    {
        [Fact]
        public void FilterTest()
        {
            var expression = "$filter=Age eq 10";
            var docQuery = new ODataQueryParser<Employee>();
            var query =  docQuery.TryParse(expression);

            Assert.NotNull(query.Filter.FilterExpression);
        }


        [Fact]
        public async Task ComplexFilterTest()
        {
            var expression = "$filter=(CompanyGroup eq 'Synechron' and IsDeleted eq false) or NumberOfEmployees ge 1";
            //var expression = "startswith(Name,'Syne') eq true";
            var runner = new MongoDBQueryRunner<Company>();
            var docQuery = new ODataQueryParser<Company>();
            var query = docQuery.TryParse(expression);
            runner.Create(query);
            var collection = GetCollection<Company>("company");
            var list = await runner.QueryAsync(collection);
            Assert.True(list.Count > 0);
        }


        private IMongoCollection<T> GetCollection<T>(string name)
        {
            var mongoUrl = new MongoUrl("mongodb://localhost:27017");
            MongoClientSettings mongoClientSettings = new MongoClientSettings
            {
                ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(e =>
                    {
                        Debug.WriteLine($"{e.DatabaseNamespace} - {e.CommandName} - {e.Command.ToJson()}");
                    });
                }
            };
            var settings = mongoClientSettings;
            MongoClient client = new MongoClient(settings);
            var convention = new ConventionPack();
            convention.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("IgnoreExtraElementConvention", convention, t => true);
            var collection = client.GetDatabase("quartz-dev").GetCollection<T>(name);
            

            return collection;
        }
    }
}
