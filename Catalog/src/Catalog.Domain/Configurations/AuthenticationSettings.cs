using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Configurations
{
    public class AuthenticationSettings
    {
        public string Secret { get; set; }
        public int ExpirationDays { get; set; }
    }
}
