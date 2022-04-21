using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Catalog.Domain.Responses;
using Catalog.Domain.Requests.User;

namespace Catalog.Domain.Services
{
    public interface IUserService
    {
        Task<UserResponse> GetUserAsync(GetUserRequest request, CancellationToken cancellationToken = default);
        Task<UserResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
        Task<TokenResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
    }
}
