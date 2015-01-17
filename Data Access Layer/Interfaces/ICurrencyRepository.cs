using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;

namespace Monei.DataAccessLayer.Interfaces
{
	public interface ICurrencyRepository
	{
		Currency Read(string code);
		Currency Create(Currency currency);
		void Delete(int id);

	}
}
