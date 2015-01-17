using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Monei.DataAccessLayer.MongoDB
{
	public class ContractRepository : MongoDBRepositoryBase<Contract>
	{
		private const string COLLECTION_NAME = "Contracts";
		private static ContractRepository instance = new ContractRepository();

		public static ContractRepository Instance { get { return instance; } }
		
		public IEnumerable<Contract> ListContracts(int accountId)
		{

			return GetCollection().Find(Query.Null).OrderBy(c => c.StartDate).ToList();

		}

		public Contract CreateContract(Contract contract)
		{
			var result = GetCollection().Insert(contract);
		
			if (result.Ok)
				return contract;

			throw new Exception("Fail to insert Contract to database. Error: " + result.ErrorMessage);
		}

		public void Delete(int contractId)
		{
			WriteConcernResult result = GetCollection().Remove(Helper.CreateQueryForId(contractId));

			if (!result.Ok)
				throw new Exception("Fail to delete record." + result.ErrorMessage);
		}

	}
}
