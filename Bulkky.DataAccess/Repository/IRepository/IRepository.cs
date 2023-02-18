using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? expression = null,string? includeProperty = null);

		public T GetFirstOrDefault(Expression<Func<T, bool>> expression, string? includeProperty = null);
        public void Add(T entity);
        public void AddRange(IEnumerable<T> entities);
        public void Remove(T entity);
        public void RemoveRange(IEnumerable<T> entities);


    }
}
