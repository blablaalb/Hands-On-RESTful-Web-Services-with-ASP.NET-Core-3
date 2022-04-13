using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Filters;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses;
using Catalog.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace Catalog.API.Tests.Filters
{
    public class ItemExistsAttributeTests
    {
        [Fact]
        public async Task Should_Continue_Pipeline_When_Id_Is_Present()
        {
            var id = Guid.NewGuid();
            var itemService = new Mock<IItemService>();

            itemService
                .Setup(x => x.GetItemAsync(It.IsAny<GetItemRequest>()))
                .ReturnsAsync(new ItemResponse { Id = id });

            var filter = new ItemExistsAttribute.ItemExistsFilterIml(itemService.Object);

            var actionExecutedContext = new ActionExecutingContext(
                new ActionContext(
                    new DefaultHttpContext()
                    , new RouteData()
                    , new ActionDescriptor()
                    )
                , new List<IFilterMetadata>()
                , new Dictionary<string, object> { { "id", id } }
                , new object()
                );

            var nextCallback = new Mock<ActionExecutionDelegate>();
            await filter.OnActionExecutionAsync(actionExecutedContext, nextCallback.Object);

            nextCallback.Verify(executinnDelegate => executinnDelegate(), Times.Once);
        }

    }
}
