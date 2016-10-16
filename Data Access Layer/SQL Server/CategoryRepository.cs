using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Interfaces;
using Monei.DataAccessLayer.SqlServer;
using Monei.Entities;
using NHibernate;
using NHibernate.Linq;

namespace Monei.DataAccessLayer.SqlServer
{
    public class CategoryRepository: RepositoryBase<int, Category>, ICategoryRepository
    {
        public CategoryRepository(ISessionFactoryProvider sessionFactoryProvider) : base(sessionFactoryProvider)
        {
        }

        public IEnumerable<Category> List()
        {
            using (ISession session = OpenSession())
            {
                return session.Query<Category>().ToList();
            }
        }

        public IEnumerable<Category> ListWithSubcategories()
        {
            using (ISession session = OpenSession())
            {
                return session.Query<Category>().Fetch(c => c.Subcategories).ToList();
            }
        }

        public new Category Create(Category item)
        {
            int id = base.Create(item);
            return Read(id);
        }

        public void MoveSubcategory(int subcategoryId, int categoryId)
        {		
            using (ISession session = OpenSession())
            {
                Subcategory subcategory = session.Get<Subcategory>(subcategoryId);
                Category category = session.Get<Category>(categoryId);
                subcategory.Category = category;
                session.SaveOrUpdate(subcategory);
                session.Flush();
            }
        }
    }
}
