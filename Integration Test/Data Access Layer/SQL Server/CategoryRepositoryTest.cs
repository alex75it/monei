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
            sessionFactory.Statistics.Clear();

            repository.List();

            sessionFactory.Statistics.CloseStatementCount.ShouldEqual(1);
        }

        [Test]
        public void AddCategory()
        {
            string name = "Transport and Parking";
            string description = "Airplane, train and bus tickets, highway tolls, parking fees.";

            var category = new Category()
            {
                Name = name,
                Description = description,
                ImageName = null,
            };

            Helper.GetTestAccount();

            // create
            category = CategoryRepository.Create(category);

            Assert.IsNotNull(category);

            Assert.AreEqual(name, category.Name, "Names are not equal.");
            Assert.AreEqual(description, category.Description, "Descriptions are not equal.");

            // cleanup
            CategoryRepository.Delete(category.Id);


            // test max length for name
            int maxLength = Category.NAME_MAX_LENGTH;

            name = new String('a', maxLength + 1);
            category.Name = name;
            try
            {
                CategoryRepository.Create(category);
            }
            catch (Exception)
            {
                //Assert.();
            }

            CategoryRepository.Delete(category.Id);
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

    }
}
