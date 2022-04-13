using Catalog.Domain.Entities;
using Catalog.Fixtures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Catalog.Domain.Requests.Item;
using System.Net.Http;
using Catalog.API.Responses;
using Catalog.Domain.Responses;
using System.Linq;

namespace Catalog.API.Tests.Controllers
{
    public class ItemControllerTests : IClassFixture<InMemoryWebApplicationFactory<Startup>>
    {
        private readonly InMemoryWebApplicationFactory<Startup> _factory;

        public ItemControllerTests(InMemoryWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/items/?pageSize=1&pageIndex=0", 1, 0)]
        [InlineData("/api/items/?pageSize=2&pageIndex=0", 2, 0)]
        [InlineData("/api/items/?pageSize=1&pageIndex=1", 1, 1)]
        public async Task Get_Should_Return_Paginated_Data(string url, int pageSize, int pageIndex)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<PaginatedItemsResponseModel<ItemResponse>>(responseContent);
            responseEntity.PageIndex.ShouldBe(pageIndex);
            responseEntity.PageSize.ShouldBe(pageSize);
            responseEntity.Data.Count().ShouldBe(pageSize);
        }

        [Theory]
        [LoadData("item")]
        public async Task Get_By_Id_Should_Return_Item_Data(Item request)
        {
            const string id = "86bff4f7-05a7-46b6-ba73-d43e2c45840f";
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/api/items/{id}");

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<Item>(responseContent);

            responseEntity.ShouldNotBeNull();
        }

        [Theory]
        [LoadData("item")]
        public async Task Add_Should_Create_New_Record(AddItemRequest request)
        {
            var client = _factory.CreateClient();

            var content = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/items", httpContent);

            response.EnsureSuccessStatusCode();
            response.Headers.Location.ShouldNotBeNull();
        }

        [Theory]
        [LoadData("item")]
        public async Task Update_Should_Modify_Existing_Item(EditItemRequest request)
        {
            var client = _factory.CreateClient();

            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/items/{request.Id}", httpContent);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<Item>(responseContent);

            responseEntity.Name.ShouldBe(request.Name);
            responseEntity.Description.ShouldBe(request.Description);
            responseEntity.GenreId.ShouldBe(request.GenreId);
            responseEntity.ArtistId.ShouldBe(request.ArtistId);
        }


    }
}
