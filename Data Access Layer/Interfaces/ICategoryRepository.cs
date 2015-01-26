using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;
using MongoDB.Bson;

namespace Monei.DataAccessLayer.Interfaces
{
	public interface ICategoryRepository
	{
		Category Create(Category item);

		Category Read(int id);

		void Update(Category item);

		void Delete(int id);

		IEnumerable<Category> List();
		IEnumerable<Category> ListWithSubcategories();

		void MoveSubcategory(int subcategoryId, int categoryId);
	}
}
