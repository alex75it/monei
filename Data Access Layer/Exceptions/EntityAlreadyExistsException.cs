using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.DataAccessLayer.Exceptions
{
	/// <summary>
	/// Exception used when somew entity cannot be saved in the storage because another entity already exists with the same oproperty.
	/// </summary>
	public class EntityAlreadyExistsException :Exception
	{
		/// <summary>
		/// The name of the property that already exists in the storage.
		/// </summary>
		public string PropertyName { get; private set; }

		public EntityAlreadyExistsException(string propertyName)
		{
			PropertyName = propertyName;
		}

	}
}
