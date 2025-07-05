using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Repositories
{
	public interface IRepository<T> where T : class, new()
	{
		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null);
		Task<T> GetByIdAsync(int id);
		Task<T> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>> include = null);
		Task Add(T entity);
		void Update(T entity);
		void Delete(int id);
		void Delete(T entity);
	}
}
