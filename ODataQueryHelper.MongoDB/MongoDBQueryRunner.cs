using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        public FilterDefinition<T> FilterDefinition { get; set; }

        /// <summary>
        /// MongoDB interpreted sort criteria
        /// </summary>
        public SortDefinition<T> SortDefinition { get; set; }

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
        public async Task<IList<T>> QueryAsync(IMongoCollection<T> mongoCollection)
        {
            if (mongoCollection == null)
            {
                throw new ArgumentNullException(nameof(mongoCollection));
            }

            if (FilterDefinition == null)
            {
                FilterDefinition = Builders<T>.Filter.Empty;
            }

            return await mongoCollection
                .Find(FilterDefinition)
                .Sort(SortDefinition)
                .Skip(Skip)
                .Limit(Limit)
                .ToListAsync();
        }

        /// <summary>
        /// Creates MongoDB friendly filter, sort , skip and limt options from <paramref name="documentQuery"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="documentQuery">Document Query generated from ODATA expression query</param>
        public void Create(DocumentQuery<T> documentQuery)
        {
            if (documentQuery == default(DocumentQuery<T>))
            {
                throw new ArgumentNullException(nameof(documentQuery));
            }

            if (documentQuery.Filter.FilterExpression != default(Expression<Func<T,bool>>))
            {
                FilterDefinition = new ExpressionFilterDefinition<T>(documentQuery.Filter.FilterExpression);
            }

            var sortBuilder = Builders<T>.Sort;
            List<SortDefinition<T>> sortDefinitions = new List<SortDefinition<T>>();

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
