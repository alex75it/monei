using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;
using MongoDB.Driver.Builders;

namespace Monei.DataAccessLayer.MongoDB
{
	public class CurrencyRepository: MongoDBRepositoryBase<Currency> //, ICurrencyRepository
    {

		public Currency GetCurrency(string code)
		{
			return GetCollection().FindOne(Query.EQ("Code", code));
		}
	
	}
}
