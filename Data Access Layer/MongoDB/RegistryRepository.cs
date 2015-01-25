using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Filters;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoRepository;

namespace Monei.DataAccessLayer.MongoDB
{
	//[CollectionName("Registry")]
	public class RegistryRepository : MongoDBRepositoryBase<RegistryRecord>, IRegistryRepository 
	{
		private const string COLLECTION_NAME = "Registry";

		public static string CollectionName { get { return COLLECTION_NAME; } }

		private static RegistryRepository instance = new RegistryRepository();

		public static RegistryRepository Instance { get { return instance; } }

		public IList<RegistryRecord> ListRecords(RegistryFilters filters)
		{
			return GetCollection().Find(GenerateQuery(filters)).OrderByDescending(r => r.Date).ToList();
			
		}

		public RegistryRecord AddRecord(RegistryRecord record)
		{
			var collection = GetCollection();

			var result = collection.Insert(record);

			if(result.Ok)
				return record;
			
			throw new Exception("Fail to insert a record. " + result.ErrorMessage);	
		}

		public void UpdateRecord(RegistryRecord record)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
			WriteConcernResult result = GetCollection().Remove (Helper.CreateQueryForId(id));

			//RegistryRecord record_1 = GetCollection().Find(Helper.CreateQueryForId(id)).First();

			//RegistryRecord record = GetCollection().FindOneById(id);
			//if(record != null)
			//{
				//WriteConcernResult result = GetCollection().Remove

				if (!result.Ok)			
					throw new Exception("Fail to delete record." + result.ErrorMessage);
			//}
		}
		
		public void DeleteRecord(int recordId)
		{
			throw new NotImplementedException();
		}
		

		public bool IsSubcategoryUsed(int subcategoryId)
		{
			return GetCollection().Count(Query.EQ(RegistryRecord.FIELD_SUBCATEGORY, subcategoryId)) > 0;
		}


		public RegistryRecord Read(int id)
		{
			return GetCollection().FindOneById(id);			
		}



		private IMongoQuery GenerateQuery(RegistryFilters filters)
		{
			filters.Normalize();

			try
			{
				IList<IMongoQuery> queries = new List<IMongoQuery>() { };

				//IMongoQuery startDateQuery = Query.GTE(RegistryRecord.FIELD_DATE, StartDate);
				//IMongoQuery endDateQuery = Query.Null;
				//IMongoQuery categoryQuery = Query.Null;

				if (filters.AccountId != null)
					queries.Add(Query.EQ(RegistryRecord.FIELD_ACCOUNT + "._id", filters.AccountId));

				if (filters.StartDate > DateTime.MinValue)
					queries.Add(Query.GTE(RegistryRecord.FIELD_DATE, filters.StartDate));

				if (filters.EndDate != DateTime.MaxValue)
					queries.Add(Query.LTE(RegistryRecord.FIELD_DATE, filters.EndDate));

				if (filters.CategoryId != 0)  // todo: ObjectId.Empty
					queries.Add(Query.EQ(RegistryRecord.FIELD_CATEGORY + "._id", filters.CategoryId));
				//queries.Add( Query.ElemMatch(RegistryRecord.FIELD_CATEGORY, Query.EQ("_id", CategoryId) ));
				//queries.Add( Query.EQ(RegistryRecord.FIELD_CATEGORY, CategoryId) );

				// todo: add filter for description
				//if (!string.IsNullOrWhiteSpace(filters.Description))
				//	queries.Add();

				// todo: add query condition for "ShowOnlyTaxDeduxtible" and "IncludeSpecialEvent".
				//if(filters.ShowOnlyTaxDeductible)
				//	queries.Add(Query.EQ(RegistryRecord))

				string json = queries.ToJson();
				//logger.InfoFormat("Mongo Query: {0}", json);

				return Query.And(queries.ToArray());
			}
			catch (Exception exc)
			{
				// todo: write filters data
				throw new Exception("Fail to generate query.", exc);
			}
		}


	}//class
}
