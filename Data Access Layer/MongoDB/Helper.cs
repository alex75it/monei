using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Monei.DataAccessLayer.MongoDB
{
	internal static class Helper
	{


		public static IMongoQuery CreateQueryForId(int id)
		{
			return Query.EQ("_id", id);
		}

	}//class
}
