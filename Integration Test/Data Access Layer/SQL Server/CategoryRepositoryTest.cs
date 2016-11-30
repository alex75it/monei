using System;
using System.Linq;
using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.Entities;
using Monei.Test.IntegrationTest.DataAccessLayer.SqlServer;
using Should;
using NUnit.Framework;
using NHibernate.Linq;

namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{
    [TestFixture, Category("Repository"), Category("Category")]
    public class CategoryRepositoryTest :RepositoryTestBase
    {
        private CategoryRepository repository;

        private ISessionFactoryProvider sessionFactoryProvider;

        [SetUp]
        public void SetUp()
        {
            sessionFactoryProvider = new SessionFactoryProvider();
            repository = new CategoryRepository(sessionFactoryProvider);
        }

        [Test]
        public void List()
        {
            IEnumerable<Category> list = repository.ListWithSubcategories();

            list.ShouldNotBeEmpty();            
        }

        [Test, Category("NHibernate")]
        public void List_should_ExecuteOnlyAQuery()
        {
            var sessionFactory = sessionFactoryProvider.GetSessionFactory();

            if (!sessionFactory.Statistics.IsStatisticsEnabled)
                Assert.Ignore("Statistics should be enabled");

            sessionFactory.Statistics.Clear();

            // Execute
            repository.List();

            sessionFactory.Statistics.CloseStatementCount.ShouldEqual(1);
        }

        [Test, Category("NHibernate")]
        public void ListWithSubcategories_should_ExecuteOnlyAQuery()
        {
            var sessionFactory = sessionFactoryProvider.GetSessionFactory();

            if (!sessionFactory.Statistics.IsStatisticsEnabled)
                Assert.Ignore("Statistics should be enabled");

            sessionFactory.Statistics.Clear();

            // Execute
            var categories = repository.ListWithSubcategories();

            categories.ToList()[0].Subcategories.ShouldNotBeEmpty();

            sessionFactory.Statistics.CloseStatementCount.ShouldEqual(1);
        }

        [Test]
        public void Create_should_CreateANewCategory()
        {
            string name = "Category";
            string description = "Category description";

            Category category = ExecuteCreateMethod(name, description);

            category.ShouldNotBeNull();
            category.Name.ShouldEqual(name, "Names are not equal.");
            category.Description.ShouldEqual(description, "Descriptions are not equal.");
        }

        [Test]
        public void UpdateCategory()
        {
            string newName = "Category B";
            string newDescription = "Description B";

            Account account = Helper.GetTestAccount();
            Category category = new Category()
            {
                Name = "Category A",
                Description = "Description B",
            };

            // Clean up data

            var categories = CategoryRepository.List().Where(c => c.Name == category.Name || c.Name == newName ).ToList();
            foreach(var c in categories)
                CategoryRepository.Delete(c.Id);

            category.CreationAccount = account;


            int categoryId = CategoryRepository.Create(category).Id;


            category = CategoryRepository.Read(categoryId);

            category.Name = newName;
            category.Description = newDescription;
            category.CreationAccount = account; //todo: set a new Account, it Creation Account MUST not change with update

            // Exceute
            CategoryRepository.Update(category);

            // Verify
            Category testCategory = CategoryRepository.Read(category.Id);
            testCategory.Name.ShouldEqual(newName);
            testCategory.Description.ShouldEqual(newDescription);
            //testCategory.CreationAccount...
        }

        #region utils
        private Category ExecuteCreateMethod(string name, string description)
        {            
            var category = new Category()
            {
                Name = name,
                Description = description,
                ImageName = null,
            };

            DeleteCategoryWithName(category.Name);

            try
            {
                // Execute
                category = CategoryRepository.Create(category);
            }
            finally
            {
                CategoryRepository.Delete(category.Id);
            }

            return category;
        }

        private void DeleteCategoryWithName(string name)
        {
            using (var session = sessionFactoryProvider.GetSessionFactory().OpenSession())
            {
                var category = session.Query<Category>().Where(c => c.Name == name).SingleOrDefault();
                if (category != null)
                    session.Delete(category);
                session.Flush();
            }
        }

        #endregion
    }
}
