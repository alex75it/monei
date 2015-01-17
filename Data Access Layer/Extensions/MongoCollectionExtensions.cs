using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Monei.DataAccessLayer.Extensions
{
	public static class MongoCollectionExtensions
	{


		public static void RemoveById(this MongoCollection collection, int id)
		{
			IMongoQuery query = Query.EQ("_id", id);
			collection.Remove(query);
		}

	}
}
