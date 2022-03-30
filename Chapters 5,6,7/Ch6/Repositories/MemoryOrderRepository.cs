using Ch6.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ch6.Repositories
{
    public class MemoryOrderRepository : IOrderRepository
    {
        private List<Order> _orders;

        public IEnumerable<Order> Get() => _orders.Where(o => !o.IsInactive).ToList();

        public MemoryOrderRepository()
        {
            _orders = new List<Order>();
        }

        public void Add(Order order)
        {
            _orders.Add(order);
        }

        public Order Delete(Guid orderId)
        {
            var target = _orders.FirstOrDefault(o => o.Id == orderId);

            target.IsInactive = true;
            Update(orderId, target);
            return target;
        }

        public Order Get(Guid orderId)
        {
            return _orders.Where(o => !o.IsInactive).FirstOrDefault(o => o.Id == orderId);
        }

        public void Update(Guid orderId, Order order)
        {
            var result = _orders.FirstOrDefault(o => o.Id == orderId);
            if (result != null) result.ItemsIds = order.ItemsIds;
        }
    }
}
