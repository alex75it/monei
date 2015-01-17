using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Monei.Entities
{
	public class Currency :EntityBase<int>
	{
		public const string EUR_CODE = "EUR";
		public const string USD_CODE = "USD";

		[BsonRequired]
		[Required]
		public virtual string Code { get; set; }
		public virtual string Name { get; set; }
		public virtual string Symbol { get; set; }

		public override int GetHashCode()
		{
			//return base.GetHashCode();
			return Id;
		}
	}
}
