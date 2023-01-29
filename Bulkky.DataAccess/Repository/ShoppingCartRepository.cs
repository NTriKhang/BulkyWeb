using Bulky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public class ShoppingCartRepository : Repository<ShoppingCart2> , IShoppingCartRepository
	{
        private readonly Application _db;
        public ShoppingCartRepository(Application db) : base(db) 
        {
            _db = db;
        }
    }
}
