using Catalog.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Services;
using Catalog.API.Filters;
using Catalog.API.Responses;
using System.Linq;
using Catalog.Domain.Responses;

namespace Catalog.API.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            this._itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageSize = 10, int pageIndex = 0)
        {
            var result = await _itemService.GetItemsAsync();

            var totalItem = result.Count();

            var itemOnPage = result
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var model = new PaginatedItemsResponseModel<ItemResponse>(pageIndex, pageSize, totalItem, itemOnPage);

            return Ok(model);
        }

        [HttpGet("{id:guid}")]
        [ItemExists]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _itemService.GetItemAsync(new GetItemRequest { Id = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddItemRequest request)
        {
            var result = await _itemService.AddItemAsync(request);
            return CreatedAtAction(nameof(GetById), new { Id = result.Id }, null);
        }

        [HttpPut("{id:guid}")]
        [ItemExists]
        public async Task<IActionResult> Put(Guid id, EditItemRequest request)
        {
            request.Id = id;
            var result = await _itemService.EditItemAsync(request);

            return Ok(result);
        }
    }
}
