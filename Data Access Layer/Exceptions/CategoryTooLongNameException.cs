using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.DataAccessLayer.Exceptions
{
	public class TooLongCategoryNameException : TooLongPropertyBaseException
	{
		public TooLongCategoryNameException(int maxLength)
			: base(maxLength)
		{ }
	}
}
