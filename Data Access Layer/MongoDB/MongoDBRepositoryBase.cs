using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoRepository;

namespace Monei.DataAccessLayer.MongoDB
{

	public abstract class MongoDBRepositoryBase<T>: IMongoDBRepository
	{
		private const string CONNECTION_STRING = "mongodb://localhost";
		private const string DATABASE_NAME = "monei";

		
		private static string CollectionName { get { 
			return typeof(T).Name;
		} }

		private static MongoClient client;

		private static MongoClient GetClient()
		{
			if(client == null)
				client = new MongoClient(CONNECTION_STRING);

			return client;
		}


		private static MongoServer GetServer()
		{
			return GetClient().GetServer();
		}

		internal static MongoDatabase GetDatabase()
		{
			return GetServer().GetDatabase(DATABASE_NAME);
		}

		protected static MongoCollection<T> GetCollection(string collectionName){

			return GetDatabase().GetCollection<T>(collectionName);
			// todo: add index if exists
			//if(Indexes.Count 
		}

		protected static MongoCollection<T> GetCollection()
		{						
			return GetCollection(CollectionName);
		}


	}//class
}
