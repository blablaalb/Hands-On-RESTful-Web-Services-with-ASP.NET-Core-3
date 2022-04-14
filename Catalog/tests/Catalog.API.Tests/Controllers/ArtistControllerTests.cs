using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Catalog.Fixtures;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Catalog.API.Responses;
using Catalog.Domain.Responses;
using Shouldly;
using Catalog.Domain.Entities;
using Catalog.Domain.Requests.Artists;
using System.Net.Http;
using System.Text;
using System.Net;

namespace Catalog.API.Tests.Controllers
{
    public class ArtistControllerTests: IClassFixture<InMemoryWebApplicationFactory<Startup>>
    {
        private readonly InMemoryWebApplicationFactory<Startup> _factory;

        public ArtistControllerTests(InMemoryWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/artist/")]
        public async Task smoke_test_on_items(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("/api/artist/?pageSize=1&pageIndex=0", 1, 0)]
        public async Task get_should_returns_paginated_data(string url, int pageSize, int pageIndex)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<PaginatedItemsResponseModel<GenreResponse>> (responseContent);

            responseEntity.PageIndex.ShouldBe(pageIndex);
            responseEntity.PageSize.ShouldBe(pageSize);
            responseEntity.Data.Count().ShouldBe(pageSize);
        }

        [Theory]
        [LoadData("artist")]
        public async Task get_by_id_should_return_right_artist(Artist request)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/api/artist/{request.ArtistId}");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<Artist>(responseContent);

            responseEntity.ArtistId.ShouldBe(request.ArtistId);
        }

        [Fact]
        public async Task add_should_create_new_artist()
        {
            var addArtistRequest = new AddArtistRequest { ArtistName = "The Braze" };
            var client = _factory.CreateClient();
            var httpContent = new StringContent(JsonConvert.SerializeObject(addArtistRequest), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/artist", httpContent);

            response.EnsureSuccessStatusCode();
            var responseHeader = response.Headers.Location;
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            responseHeader.ToString().ShouldContain("/api/artist/");
        }

    }
}
