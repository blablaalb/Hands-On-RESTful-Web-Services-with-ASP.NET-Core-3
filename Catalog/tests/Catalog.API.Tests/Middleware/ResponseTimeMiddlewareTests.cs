using Catalog.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace Catalog.API.Tests.Middleware
{
    public class ResponseTimeMiddlewareTests: IClassFixture<InMemoryWebApplicationFactory<Startup>>
    {
        private readonly InMemoryWebApplicationFactory<Startup> _factory;

        public ResponseTimeMiddlewareTests(InMemoryWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/items/?pageSize=1&pageIndex=0")]
        [InlineData("/api/artist/?pageSize=1&pageIndex=0")]
        [InlineData("/api/genre/?pageSize=1&pageIndex=0")]
        public async Task Middleware_Should_Set_The_Correct_Response_Time_Custom_Header(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            response.Headers.GetValues("X-Response-Time-ms").ShouldNotBeEmpty();
        }

    }
}
