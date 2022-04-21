using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Xunit;
using Catalog.Fixtures;
using Catalog.Domain.Requests.User;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Shouldly;
using Catalog.Domain.Responses;
using System.Net.Http.Headers;

namespace Catalog.API.Tests.Controllers
{
    public class UserControllerTests : IClassFixture<InMemoryWebApplicationFactory<Startup>>
    {
        private readonly InMemoryWebApplicationFactory<Startup> _factory;

        public UserControllerTests(InMemoryWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/user/auth")]
        public async Task Sign_In_Should_Retrieve_a_Token(string url)
        {
            var client = _factory.CreateClient();
            var request = new SignInRequest { Email = "test@test.com", Password = "password" };
            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, httpContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            responseContent.ShouldNotBeEmpty();
        }

        [Theory]
        [InlineData("/api/user/auth")]
        public async Task Sign_In_Should_Retrieve_Bad_Request_With_Invalid_Password(string url)
        {
            var client = _factory.CreateClient();
            var request = new SignInRequest { Email = "test@test.com", Password= "passworddd" };
            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, httpContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseContent.ShouldNotBeEmpty();
        }

        [Theory]
        [InlineData("/api/user")]
        public async Task Get_With_Authorized_User_Should_Retrieve_The_Right_User(string url)
        {
            var client = _factory.CreateClient();
            var signInRequest = new SignInRequest { Email = "test@test.com", Password = "password" };
            var httpContent = new StringContent(JsonConvert.SerializeObject(signInRequest), Encoding.UTF8, "application/json"); 
            var response = await client.PostAsync(url + "/auth", httpContent);
            string responseContent = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);
            var restrictedResponse = await client.GetAsync(url);

            restrictedResponse.EnsureSuccessStatusCode();
            restrictedResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

    }
}
