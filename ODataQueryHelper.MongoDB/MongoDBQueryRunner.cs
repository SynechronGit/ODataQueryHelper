using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ODataQueryHelper.Core.Model;

namespace ODataQueryHelper.MongoDB
{
    public class MongoDBQueryRunner<T> : IMongoDBQueryRunner<T> where T: class
	{
		public FilterDefinition<BsonDocument> FilterDefinition { get; set; }

		public SortDefinition<BsonDocument> SortDefinition { get; set; }

		public IList<T> Query(IMongoCollection<BsonDocument> mongoCollection)
		{
			if (mongoCollection == null)
			{
				throw new ArgumentNullException(nameof(mongoCollection));
			}
            //var filterDef = new FilterDefinitionBuilder<BsonDocument>().Eq("Age", "10");

            var list = mongoCollection
                .Find(FilterDefinition)
                //.Sort(SortDefinition)
                .ToList();
            return list.Select(d => BsonSerializer.Deserialize<T>(d))
				.ToList();

		}

		public void Create(DocumentQuery parser)
		{
			if (parser == default(DocumentQuery))
			{
				throw new ArgumentNullException(nameof(parser));
			}

			//TODO : Get Root node and keep adding filter
			FilterCriteriaNode node = parser.Filter.Root.Nodes.First();
			//TODO : Add Validation
			FilterDefinition = new FilterDefinitionBuilder<BsonDocument>().Eq(node.PropertyName, node.ValueToCheck);


			var sortBuilder = Builders<BsonDocument>.Sort;
			List<SortDefinition<BsonDocument>> sortDefinitions = new List<SortDefinition<BsonDocument>>();

			parser.OrderBy.OrderByFields.ForEach(of =>
			{
				switch (of.Direction)
				{
					case OrderDirectionType.Ascending:
						sortDefinitions.Add(sortBuilder.Ascending(of.FieldName));
						break;
					case OrderDirectionType.Descending:
						sortDefinitions.Add(sortBuilder.Descending(of.FieldName));
						break;
				}

			});

			 SortDefinition = sortBuilder.Combine(sortDefinitions);
			
		}
	}
}
