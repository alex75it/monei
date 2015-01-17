using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;

namespace Monei.DataAccessLayer.Interfaces
{
	public interface ISubcategoryRepository : IRepository<int, Subcategory>
	{
		List<Subcategory> List(int categoryId);
	}
}
