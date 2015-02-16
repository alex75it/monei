using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Monei.Entities
{
	public class Category :EntityBase<int>
	{
		public const int NAME_MIN_LENGTH = 1;
		public const int NAME_MAX_LENGTH = 25;
		public const int DESCRIPTION_MAX_LENGTH = 1000;

		public string ImageName { get; set; }

	
		public string Name { get; set; }	
		public string Description { get; set; }
		public DateTime? DeletionDate { get; set; }


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
			return obj is Category && ((Category)obj).Id == Id;
		}

	}
}
