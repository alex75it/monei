using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;
using NHibernate;

namespace Monei.DataAccessLayer.SqlServer
{
    public class AbstractRepository<TKey,TEntity> :RepositoryBase<TKey, TEntity> where TEntity:EntityBase<TKey>
    {
        

    }
}
