using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using ODataQueryHelper.Core.Model;

namespace ODataQueryHelper.MongoDB
{
    public interface IMongoDBQueryRunner<T> where T : class
    {
        void Create(DocumentQuery parser);
        IList<T> Query(IMongoCollection<BsonDocument> mongoCollection);
    }
}