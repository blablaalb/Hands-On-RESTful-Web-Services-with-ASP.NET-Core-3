using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Catalog.Domain.Entities
{
    public class User: IdentityUser
    {
        public string Name { get; set; }
    }
}
