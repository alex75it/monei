using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.DataAccessLayer.Exceptions
{
	public class CategoryTooLongNameException : TooLongPropertyBaseException
	{
		public CategoryTooLongNameException(int maxLength)
			: base(maxLength)
		{ }
	}
}
