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

		////[DataMember(IsRequired = true)]
		//public int CategoryID { get; set; }

		//public Subcategory()
		//{
		//	LastChangeDate = null;
		//}


		public Category Category { get; set; }
		
		//[DataMember(IsRequired = true)]		
		[StringLength(NAME_MAX_LENGTH), Required]
		public string Name { get; set; }
		//[DataMember(IsRequired = true)]
		public string Description { get; set; }


		public override int GetHashCode()
		{
			return Id;
		}

		public override bool Equals(object obj)
		{
			return ((Subcategory)obj).Id == Id;
		}

	}//class
}
