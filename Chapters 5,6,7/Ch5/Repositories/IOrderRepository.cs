using Ch5.Models;
using System;
using System.Collections.Generic;

namespace Ch5.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Get();
        Order Get(Guid orderId);
        void Add(Order order);
        void Update(Guid orderId, Order order);
        Order Delete(Guid orderId);
    }
}
