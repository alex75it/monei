using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Core.DataAnalysis.BusinessObjects;
using Monei.Entities;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.Core.DataAnalysis.BusinessObjects
{
	[TestFixture]
	public class CategoriesDataTest
	{
		[Test]
		public void Ctor()
		{
			var data = new CategoriesData();

			data.Categories.ShouldBeEmpty();
			data.List.ShouldBeEmpty();
		}

		[Test]
		public void AddValue_Should_AddACategory()
		{
			int categoryId = 123;
			Category category = new Category();
			decimal amount = 45.67m;
			CategoryData categoryData = new CategoryData(category, amount);

			var data = new CategoriesData();

			data.AddValue(categoryId, categoryData);

			data.Categories.ShouldNotBeEmpty();
			data.Categories[0].ShouldBeSameAs(categoryData);
		}


		[Test]
		public void A()
		{
			IEnumerable<Category> categories = new List<Category>()
			{
				new Category() { Id=1, Name="A" },
				new Category() { Id=2, Name="B" }
			};

			var data = new CategoriesData();
			data.SetAllCategories(categories);

			data.List.Count.ShouldEqual(categories.Count);
		}
	}
}
