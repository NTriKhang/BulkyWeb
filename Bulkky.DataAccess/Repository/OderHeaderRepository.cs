using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly Application _db;
        public OrderHeaderRepository(Application db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.orderHeaders.Update(obj);
        }
        public void UpdateStatus(OrderHeader obj, string orderStatus, string? paymentStatus = null)
        {
            obj.OrderStatus = orderStatus;
            if (paymentStatus != null)
            {
                obj.PaymentStatus = paymentStatus;
            }
        }
		public void UpdateStripePaymentStatus(OrderHeader obj, string sessionId, string paymentIntendId)
        {
            obj.SessionId = sessionId;
            obj.PaymentIntendId = paymentIntendId;
        }
	}
}
