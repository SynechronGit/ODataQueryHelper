using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using ODataQueryHelper.Core.Model;

namespace ODataQueryHelper.MongoDB
{
    /// <summary>
    /// Creates and Runs OData query on MongoDB Collection
    /// </summary>
    /// <typeparam name="T">Document Entity</typeparam>
    public interface IMongoDBQueryRunner<T> where T : class
    {
        /// <summary>
        /// Creates MongoDB friendly filter, sort , skip and limt options from <paramref name="documentQuery"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="documentQuery">Document Query generated from ODATA expression query</param>
        void Create(DocumentQuery parser);
        /// <summary>
        /// Queries on mongoDB collection and returns list of documents.
        /// </summary>
        /// <param name="mongoCollection">MongoDB Collection</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Document List based on filter and sort definition.</returns>
        IList<T> Query(IMongoCollection<BsonDocument> mongoCollection);
    }
}