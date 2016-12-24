using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monei.Core.DataAnalysis.BusinessObjects;
using Monei.Entities;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.Core.DataAnalysis.BusinessObjects
{
    [TestFixture, Category("Category")]
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
        public void AddNewCategories_should_AddNewCatyegories()
        {
            IEnumerable<Category> categories = new List<Category>()
            {
                new Category() { Id=1, Name="A" },
                new Category() { Id=2, Name="B" }
            };

            var data = new CategoriesData();
            data.AddNewCategories(categories);

            data.List.Count.ShouldEqual(categories.Count());
        }

        [Test]
        public void AddNewCategories_when_CategoriesAlreadyExists_should_NotAddCategory()
        {
            IEnumerable<Category> categories = new List<Category>()
            {
                new Category() { Id=1, Name="A" },
                new Category() { Id=2, Name="B" }
            };

            var data = new CategoriesData();


            data.AddNewCategories(categories); // first time
            data.AddNewCategories(categories); // second time

            data.List.Count.ShouldEqual(categories.Count());
        }
    }
}
