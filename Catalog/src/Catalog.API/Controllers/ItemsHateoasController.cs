using Catalog.API.Filters;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiskFirst.Hateoas;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Catalog.API.Responses;
using System;
using Catalog.Domain.Requests.Item;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JsonException]
    public class ItemsHateoasController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILinksService _linkService;

        public ItemsHateoasController(IItemService itemService, ILinksService linkService)
        {
            _itemService = itemService;
            _linkService = linkService;
        }

        [HttpGet(Name =nameof(Get))]
        public async Task<IActionResult> Get([FromQuery] int pageSize =10, [FromQuery] int pageIndex = 0)
        {
            var result = await _itemService.GetItemsAsync();

            var totalItems = result.Count();

            var itemsOnPage = result
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            var hateoasResults = new List<ItemHateoasResponse>();

            foreach (var itemResponse in itemsOnPage)
            {
                var hateoasResponse = new ItemHateoasResponse { Data = itemResponse };
                await _linkService.AddLinksAsync(hateoasResponse);

                hateoasResults.Add(hateoasResponse);
            }

            var model = new PaginatedItemsResponseModel<ItemHateoasResponse>(pageIndex, pageSize,totalItems, hateoasResults);

            return Ok(model);
        }

        [HttpGet("{id:guid}", Name =nameof(GetById))]
        [ItemExists]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result  = await _itemService.GetItemAsync(new GetItemRequest { Id = id });
            var hateoasResult = new ItemHateoasResponse { Data = result};
            await _linkService.AddLinksAsync(hateoasResult);

            return Ok(hateoasResult);
        }

        [HttpPost(Name =nameof(Post))]
        public async Task<IActionResult> Post(AddItemRequest request)
        {
            var result = await _itemService.AddItemAsync(request);
            return CreatedAtAction(nameof(GetById), new {id = result.Id});
        }

        [HttpPut("{id:guid}", Name=nameof(Put))]
        [ItemExists]
        public async Task<IActionResult> Put(Guid id, EditItemRequest request)
        {
            request.Id = id;
            var result = await _itemService.EditItemAsync(request);

            var hateoasResult = new ItemHateoasResponse { Data = result};
            await _linkService.AddLinksAsync(hateoasResult);

            return Ok(hateoasResult);
        }

        [HttpDelete("{id:guid}", Name =nameof(Delete))]
        [ItemExists]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = new DeleteItemRequest { Id = id };
            await _itemService.DeleteItemAsync(request);
            return NoContent();
        }

    }
}
