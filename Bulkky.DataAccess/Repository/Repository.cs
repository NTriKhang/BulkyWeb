using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly Application _db;
        internal DbSet<T> dbSet;
        public Repository(Application db)
        {
            //_db.products.Include(u => u.Category).Include(u => u.CoverType);
            _db = db;
            dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public IEnumerable<T> GetAll(string? includeProperty = null)
        {
            IQueryable<T> query = dbSet;
            if(includeProperty != null)
            {
                foreach(var includeProp in includeProperty.Split(","))
                {
					query = query.Include(includeProp.Trim());
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> expression, string? includeProperty = null)
        {
            IQueryable<T> query = dbSet.Where(expression);
			if (includeProperty != null)
			{
				foreach (var includeProp in includeProperty.Split(","))
				{
					query = query.Include(includeProp.Trim());
				}
			}
			return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
