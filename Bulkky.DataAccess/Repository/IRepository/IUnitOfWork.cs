using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get; }
        public ICoverTypeRepository CoverType { get; }
        public IProductRepository Product { get; }
        public ICompanyRepository Company { get;}
        public IShoppingCartRepository shoppingCart { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailRepository OrderDetail { get;}
        public IApplicationUserRepository applicationUser { get; }
        public void Save();
    }
}
