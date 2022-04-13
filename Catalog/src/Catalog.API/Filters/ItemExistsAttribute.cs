using Catalog.Domain.Requests.Item;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Catalog.API.Filters
{
    public class ItemExistsAttribute : TypeFilterAttribute
    {
        public ItemExistsAttribute() : base(typeof(ItemExistsFilterIml))
        {
        }
        public class ItemExistsFilterIml : IAsyncActionFilter
        {
            private readonly IItemService _itemService;

            public ItemExistsFilterIml(IItemService itemService)
            {
                this._itemService = itemService;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!(context.ActionArguments["id"] is Guid id))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                var result = await _itemService.GetItemAsync(new GetItemRequest { Id = id });

                if (result == null)
                {
                    context.Result = new NotFoundObjectResult($"Item with id {id} not exist.");
                    return;
                }

                await next();
            }
        }
    }
}
