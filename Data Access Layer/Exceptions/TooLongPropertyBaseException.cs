using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.DataAccessLayer.Exceptions
{
	[Serializable]
	public class TooLongPropertyBaseException :Exception
	{
		public int MaxLength { get; private set; }

		public TooLongPropertyBaseException(int maxLength)
		{
			MaxLength = maxLength;
		}
	}
}
