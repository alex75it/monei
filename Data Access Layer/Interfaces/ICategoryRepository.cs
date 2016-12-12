using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;

namespace Monei.DataAccessLayer.Interfaces
{
	public interface ICategoryRepository
	{
		Category Create(Category item);

		Category Read(int id);

		void Update(Category item);

		void Delete(int id);

		IEnumerable<Category> List();

		void MoveSubcategory(int subcategoryId, int categoryId);
	}
}
