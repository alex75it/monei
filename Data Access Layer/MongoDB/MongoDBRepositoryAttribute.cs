using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Monei.DataAccessLayer.MongoDB
{
	public class MongoDBRepositoryAttribute :Attribute
	{
		public string CollectionName { get; set; }
	}
}
