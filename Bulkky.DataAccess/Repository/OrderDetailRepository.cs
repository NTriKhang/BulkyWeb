using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly Application _db;
        public OrderDetailRepository(Application db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetail obj)
        {
            _db.orderDetail.Update(obj);
        }
    }
}
