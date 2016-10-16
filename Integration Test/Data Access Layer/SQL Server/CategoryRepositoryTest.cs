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
        public void Create_should_CreateANewEntity()
        {
            string name = "Transport and Parking";
            string description = "Airplane, train and bus tickets, highway tolls, parking fees.";

            Category category = ExecuteCreateMethod(name, description);

            category.ShouldNotBeNull();
        }

        [Test]
        public void Create_when_NameIsTooLong_should_RaiseASpecificException()
        {
            // test max length for name
            int maxLength = Category.NAME_MAX_LENGTH;
            string name = new String('a', maxLength + 1);
            string description = "description";
                        
            Assert.Throws<ArgumentException>( () => ExecuteCreateMethod(name, description));
        }

        [Test]
        public void UpdateCategory()
        {
            string changedName = "Test B";
            string changedDescription = "Description B";

            Account account = Helper.GetTestAccount();
            Category category = new Category()
            {
                Name = "Test A",
                Description = "aaa bbb",
            };

            // Clean up data

            var categories = CategoryRepository.List().Where(c => c.Name == category.Name || c.Name == changedName ).ToList();
            foreach(var c in categories)
                CategoryRepository.Delete(c.Id);

            category.CreationAccount = account;


            int categoryId = CategoryRepository.Create(category).Id;


            category = CategoryRepository.Read(categoryId);

            category.Name = changedName;
            category.Description = changedDescription;
            category.CreationAccount = account; //todo: set a new Account, it Creation Account MUST not change with update

            // Exceute
            CategoryRepository.Update(category);

            // Verify
            Category testCategory = CategoryRepository.Read(category.Id);
            testCategory.Name.ShouldEqual(changedName);
            testCategory.Description.ShouldEqual(changedDescription);
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

            try
            {
                // Execute
                category = CategoryRepository.Create(category);

                category = CategoryRepository.Read(category.Id);

                Assert.IsNotNull(category);
                Assert.AreEqual(name, category.Name, "Names are not equal.");
                Assert.AreEqual(description, category.Description, "Descriptions are not equal.");
            }
            finally
            {
                // cleanup
                CategoryRepository.Delete(category.Id);
            }

            return category;
        }

        #endregion
    }
}
