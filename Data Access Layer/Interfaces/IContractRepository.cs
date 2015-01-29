using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;


namespace Monei.DataAccessLayer.Interfaces
{
	public interface IContractRepository :IRepository<int, Contract>
	{

		IEnumerable<Contract> ListContracts(int accountId);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="accountId">Account ID of whom execute the operation</param>
		/// <param name="contract"></param>
		/// <returns></returns>
		Contract CreateContract(int accountId, Contract contract);

		
		//void Delete(int contractId);
	}
}
