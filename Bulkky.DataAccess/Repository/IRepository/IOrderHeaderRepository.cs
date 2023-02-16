using Bulky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        public void Update(OrderHeader obj);
        public void UpdateStatus(OrderHeader obj, string orderStatus, string? paymentStatus=null);
        public void UpdateStripePaymentStatus(OrderHeader obj, string sessionId, string paymentIntendId);
    }
}
