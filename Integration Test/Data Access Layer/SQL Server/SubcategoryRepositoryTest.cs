using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Exceptions;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using NHibernate;
using NUnit.Framework;
using Should;
using Assert = NUnit.Framework.Assert;

namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{
    [TestFixture, Category("Data Access Layer"), Category("Subcategory")]
    public class SubcategoryRepositoryTest :RepositoryTestBase
    {
        [Test]
        public void List()
        {

            int categoryId;

            Category category = Helper.GetRandomCategory();
            categoryId = category.Id;

            List<Subcategory> list = Helper.SubcategoryRepository.List(categoryId);

            if(list.Count > 0)
            {
                Assert.IsTrue(list.All(s => s.Category.Equals(category)), "Category");
            }
            else
            {
                Assert.Inconclusive("None subcategory found for category " + category.Name);
            }
        }


        [Test]
        public void Create()
        {
            Account account = Helper.GetTestAccount();
            Category category = Helper.GetRandomCategory();

            string name = "ainjefgnq";
            string description = "aSG òLERF PLIKMERF";

            Subcategory subcategory = new Subcategory() { 
                CreationAccount = account,
                CreationDate = DateTime.Now,
                Name = name,
                Category = category,
                Description = description,				
            };

            subcategory.Id = Helper.SubcategoryRepository.Create(subcategory);

            subcategory = Helper.SubcategoryRepository.Read(subcategory.Id);

            Assert.IsNotNull(subcategory);
            Assert.AreEqual(name, subcategory.Name);
            Assert.AreEqual(description, subcategory.Description);
            

            // cleanup
            Helper.SubcategoryRepository.Delete(subcategory.Id);
        }

        [Test]
        public void Read()
        {
            string name = "to be deleted";
            string description = "<fghe e e egeavb4wt h";
            Category category = Helper.GetRandomCategory();
            int subcategoryId = 0;

            try
            {
                Subcategory subcategory = new Subcategory();
                subcategory.Name = name;
                subcategory.Description = description;
                subcategory.Category = category;
                subcategory.CreationAccount = Helper.GetTestAccount();

                subcategoryId = Helper.SubcategoryRepository.Create(subcategory);

                // Execute
                subcategory = Helper.SubcategoryRepository.Read(subcategoryId);

                // Verify
                subcategory.Name.ShouldEqual(name);
                subcategory.Description.ShouldEqual(description);
                subcategory.Category.ShouldEqual(category);
                //subcategory.CreationAccount.ShouldNotBeNull("CreationAccount is null"); not used in this entity
                Assert.IsNotNull(subcategory.CreationDate); 
                subcategory.LastChangeDate.ShouldBeNull();
                subcategory.LastUpdateAccount.ShouldBeNull();				
            }
            finally {
                if(subcategoryId != 0)
                    Helper.SubcategoryRepository.Delete(subcategoryId);
            }
        }

        [Test]
        public void Update()
        {
            string name = "Initial name";
            string description = "Initial Description";

            Account account = Helper.GetTestAccount();
            Category category = Helper.GetRandomCategory();

            Subcategory subcategory = new Subcategory() { 
                Name = name,
                Description = description,
                CreationAccount = account,
                Category = category,
                
            };
            
            // create
            subcategory.Id = Helper.SubcategoryRepository.Create(subcategory);

            // change properties
            name = "Update name  - ojear a";
            description = "Update Description  - poear òokeokff 35448re";

            subcategory.Name = name;
            subcategory.Description = description;


            // update
            Helper.SubcategoryRepository.Update(subcategory);
            
            Assert.AreEqual(name, subcategory.Name);
            Assert.AreEqual(description, subcategory.Description);
            Assert.AreEqual(category.Id, subcategory.Category.Id);
            Assert.AreEqual(account, subcategory.CreationAccount);

            // cleanup
            Helper.SubcategoryRepository.Delete(subcategory.Id);
        }

        [Test]
        public void Delete()
        {
            Subcategory subcategory = Helper.CreateRandomSubcategory();
            int subcategoryId = Helper.SubcategoryRepository.Create(subcategory);

            Helper.SubcategoryRepository.Delete(subcategoryId);

            string sqlQuery = "Select * from Subcategory where Id = " + Helper.SubcategoryRepository;

            subcategory = Helper.SubcategoryRepository.Read(subcategoryId);

            Assert.IsNull(subcategory);
        }
    }
}
