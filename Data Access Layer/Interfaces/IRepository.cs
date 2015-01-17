using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.DataAccessLayer.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TEntity">Entity managed by this repository</typeparam>
	public interface IRepository<TKey, TEntity>
	{
		TKey Create(TEntity entity);
		TEntity Read(TKey key);
		void Update(TEntity entity);
		void Delete(TKey key);
	}
}
