using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Entities
{
	public class Subcategory :EntityBase<int>
	{
		public const int NAME_MAX_LENGTH = 25;
		public const int DESCRIPTION_MAX_LENGTH = 1000;


		public Category Category { get; set; }
		
		[StringLength(NAME_MAX_LENGTH), Required]
		public string Name { get; set; }
		public string Description { get; set; }


		public override int GetHashCode()
		{
			return Id;
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj.GetHashCode() == GetHashCode();
		}

	}//class
}
