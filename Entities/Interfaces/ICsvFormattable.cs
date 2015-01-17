using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Entities.Interfaces
{
	public interface ICsvFormattable
	{
		string CreateHeadersRow();
		string CreateRow();
	}
}
