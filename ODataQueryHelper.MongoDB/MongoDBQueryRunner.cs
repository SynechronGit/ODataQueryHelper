using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ODataQueryHelper.Core.Model;

namespace ODataQueryHelper.MongoDB
{
    /// <summary>
    /// Creates and Runs OData query on MongoDB Collection
    /// </summary>
    /// <typeparam name="T">Document Entity</typeparam>
    public class MongoDBQueryRunner<T> : IMongoDBQueryRunner<T> where T : class
    {
        /// <summary>
        /// MongoDB interpreted filter criteria
        /// </summary>
        public FilterDefinition<BsonDocument> FilterDefinition { get; set; }

        /// <summary>
        /// MongoDB interpreted sort criteria
        /// </summary>
        public SortDefinition<BsonDocument> SortDefinition { get; set; }

        /// <summary>
        /// Documents to skip
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Documents to limit in collection
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Queries on mongoDB collection and returns list of documents.
        /// </summary>
        /// <param name="mongoCollection">MongoDB Collection</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Document List based on filter and sort definition.</returns>
        public IList<T> Query(IMongoCollection<BsonDocument> mongoCollection)
        {
            if (mongoCollection == null)
            {
                throw new ArgumentNullException(nameof(mongoCollection));
            }

            if (FilterDefinition == null)
            {
                FilterDefinition = Builders<BsonDocument>.Filter.Empty;
            }
            var result = mongoCollection
                .Find(FilterDefinition)
                .Sort(SortDefinition)
                .Skip(Skip)
                .Limit(Limit)
                .ToList();

            return result
                .Select(d => BsonSerializer.Deserialize<T>(d))
                .ToList();

        }

        /// <summary>
        /// Creates MongoDB friendly filter, sort , skip and limt options from <paramref name="documentQuery"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="documentQuery">Document Query generated from ODATA expression query</param>
        public void Create(DocumentQuery documentQuery)
        {
            if (documentQuery == default(DocumentQuery))
            {
                throw new ArgumentNullException(nameof(documentQuery));
            }

            if (documentQuery.Filter.Root.Nodes.Any() || documentQuery.Filter.Root.Branches.Any())
            {
                //TODO : Get Root node and keep adding filter
                FilterCriteriaNode node = documentQuery.Filter.Root.Nodes.First();
                //TODO : Add Validation
                FilterDefinition = new FilterDefinitionBuilder<BsonDocument>().Eq(node.PropertyName, node.ValueToCheck);
            }

            var sortBuilder = Builders<BsonDocument>.Sort;
            List<SortDefinition<BsonDocument>> sortDefinitions = new List<SortDefinition<BsonDocument>>();

            documentQuery
                .OrderBy
                .OrderByNodes
                .OrderBy(o => o.Sequence)
                .ToList()
                .ForEach(of =>
                {
                    switch (of.Direction)
                    {
                        case OrderByDirectionType.Ascending:
                            sortDefinitions.Add(sortBuilder.Ascending(of.PropertyName));
                            break;
                        case OrderByDirectionType.Descending:
                            sortDefinitions.Add(sortBuilder.Descending(of.PropertyName));
                            break;
                    }

                });

            SortDefinition = sortBuilder.Combine(sortDefinitions);

            Skip = documentQuery.Skip;

            Limit = documentQuery.Top;
        }
    }
}
