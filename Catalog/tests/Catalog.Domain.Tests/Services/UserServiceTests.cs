using Catalog.Domain.Configurations;
using Catalog.Domain.Requests.User;
using Catalog.Domain.Services;
using Catalog.Fixtures;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace Catalog.Domain.Tests.Services
{
    public class UserServiceTests : IClassFixture<UserContextFactory>
    {
        private readonly UserService _userService;

        public UserServiceTests(UserContextFactory factory)
        {
            _userService = new UserService(factory.InMemoryUserManager, Options.Create(
                new AuthenticationSettings { Secret = "Very secret key-word to match", ExpirationDays = 7 }));
        }

        [Fact]
        public async Task Signin_With_Invalid_User_Should_Return_a_Valid_Token_Response()
        {
            var result = await _userService.SignInAsync(new SignInRequest { Email = "invalid.user", Password = "invalid_password" });
            result.ShouldBeNull();
        }

        [Fact]
        public async Task Signin_With_Valid_User_Should_Return_a_Valid_Token_Response()
        {
            var result = await _userService.SignInAsync(new SignInRequest { Email = "test@test.com", Password = "password" });
            result.Token.ShouldNotBeEmpty();
        }

    }


}
