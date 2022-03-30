using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Ch6.Models;
using Ch6.Repositories;
using Ch6.Requests;
using Ch6.CustomRouting;

namespace Ch6.Controllers
{
    [CustomOrdersRoute]
    [ApiController]
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
        public IActionResult Put(Guid id, OrderRequest request)
        {
            if (request.ItemsIds == null) return BadRequest();
            var order = _orderRepository.Get(id);
            if (order == null)
                return NotFound(new { Messsage = $"Item with id {id} not exist." });
            order.ItemsIds = request.ItemsIds;
            _orderRepository.Update(id, order);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var order = _orderRepository.Get(id);

            if (order == null) return NotFound(new { Message = $"Item with id {id} not exist." });
            _orderRepository.Delete(id);
            return NoContent();
        }

        public IActionResult Patch(Guid id, JsonPatchDocument<Order> requestOp)
        {
            var order = _orderRepository.Get(id);
            if (order == null)
                return NotFound(new { Messge = $"Item with id {id} not exist." });

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
