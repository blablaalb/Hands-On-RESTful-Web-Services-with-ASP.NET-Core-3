using Ch7.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Ch7.Filters
{
    public class OrderExistsAttribute: TypeFilterAttribute
    {
        public OrderExistsAttribute(): base(typeof(OrderExistsFilterIml))
        {
        }

        private class OrderExistsFilterIml : IAsyncActionFilter
        {
            private readonly IOrderRepository _orderRepository;

            public OrderExistsFilterIml(IOrderRepository orderRepository)
            {
                this._orderRepository = orderRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!context.ActionArguments.ContainsKey("id"))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                if (!(context.ActionArguments["id"] is Guid id))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                var result = _orderRepository.Get(id);
                if (result == null)
                {
                    context.Result = new NotFoundObjectResult(new { Message = $"Item with id {id} not exist." });
                    return;
                }

                await next();
            }
        }

    }
}
