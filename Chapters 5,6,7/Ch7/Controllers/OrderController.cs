using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Ch7.Models;
using Ch7.Repositories;
using Ch7.Requests;
using Ch7.CustomRouting;
using Ch7.Filters;

namespace Ch7.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [CustomOrdersRoute]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Map(_orderRepository.Get()));
        }

        [HttpGet("{id:guid}")]
        [OrderExists]
        public IActionResult GetById(Guid id)
        {
            return Ok(Map(_orderRepository.Get(id)));
        }

        [HttpPost]
        public IActionResult Post(OrderRequest request)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                ItemsIds = request.ItemsIds
            };
            _orderRepository.Add(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, null);
        }

        [HttpPut("{id:guid}")]
        [OrderExists]
        public IActionResult Put(Guid id, OrderRequest request)
        {
            var order = _orderRepository.Get(id);
            order.ItemsIds = request.ItemsIds;
            _orderRepository.Update(id, order);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [OrderExists]
        public IActionResult Delete(Guid id)
        {
            var order = _orderRepository.Get(id);
            _orderRepository.Delete(id);
            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        [OrderExists]
        public IActionResult Patch(Guid id, JsonPatchDocument<Order> requestOp)
        {
            var order = _orderRepository.Get(id);
            requestOp.ApplyTo(order);
            _orderRepository.Update(id, order);
            return Ok();
        }

        private Order Map(OrderRequest request)
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                Currency = request.Currency,
                ItemsIds = request.ItemsIds
            };
        }

        private Order Map(OrderRequest request, Order order)
        {
            order.ItemsIds = request?.ItemsIds;
            order.Currency = request?.Currency;

            return order;
        }

        private IEnumerable<OrderResponse> Map(IEnumerable<Order> orders)
        {
            return orders.Select(Map).ToList();
        }

        private OrderResponse Map(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                ItemsIds = order.ItemsIds,
                Currency = order.Currency
            };
        }

    }
}
