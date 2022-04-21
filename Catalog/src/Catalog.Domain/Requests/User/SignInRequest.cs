using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Requests.User
{
    public class SignInRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
