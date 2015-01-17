using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;
using MongoDB.Bson;

namespace Monei.DataAccessLayer.Interfaces
{
	public interface IContractRepository
	{

		IEnumerable<Contract> ListContracts(ObjectId accountId);

		Contract CreateContract(ObjectId accountId, Contract contract);


	}
}
