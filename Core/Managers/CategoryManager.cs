using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Exceptions;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;

namespace Monei.Core.Managers
{
	public class CategoryManager
	{
		private readonly ICategoryRepository categoryRepository;

		public CategoryManager(ICategoryRepository categoryRepository)
		{
			this.categoryRepository = categoryRepository;
		}

		public void Update(Category category)
		{
			if (category.Name.Length > Category.NAME_MAX_LENGTH)
				throw new CategoryTooLongNameException(Category.NAME_MAX_LENGTH);
			if (category.Name.Length < Category.NAME_MIN_LENGTH)
				throw new Exception("Name is too short, min length is " + Category.NAME_MIN_LENGTH );
			if (category.Description.Length > Category.DESCRIPTION_MAX_LENGTH)
				throw new Exception("Description is too long, max length: " + Category.DESCRIPTION_MAX_LENGTH);

			categoryRepository.Update(category);
		}

	}
}
