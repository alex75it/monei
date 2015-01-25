using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Monei.Entities
{
	public class Category :EntityBase<int>
	{
		public const int NAME_MIN_LENGTH = 1;
		public const int NAME_MAX_LENGTH = 25;
		public const int DESCRIPTION_MAX_LENGTH = 1000;

		public string ImageName { get; set; }

		[BsonRequired]		
		[StringLength(NAME_MAX_LENGTH), Required]
		public string Name { get; set; }	
		[Required]
		public string Description { get; set; }
		public DateTime? DeletionDate { get; set; }

		//public List<LocalizedNameAndDescription> NameAndDescriptions { get; set; }
		//public IEnumerable<Subcategory> Subcategories { get; set; }
		public IList<Subcategory> Subcategories { get; set; }

		public Category()
		{
			Subcategories = new List<Subcategory>();
		}


		public override int GetHashCode()
		{
			return Id;
		}

		public override bool Equals(object obj)
		{
			return ((Category)obj).Id == Id;
		}

	}
}
