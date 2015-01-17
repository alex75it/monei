using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.DataAccessLayer.Exceptions
{
	public class EntityAlreadyExistsException :Exception
	{
		public string PropertyName { get; private set; }

		public EntityAlreadyExistsException(string propertyName)
		{
			PropertyName = propertyName;
		}

	}
}
