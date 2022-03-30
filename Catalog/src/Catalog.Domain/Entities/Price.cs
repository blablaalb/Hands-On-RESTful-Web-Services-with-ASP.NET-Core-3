using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Entities
{
    public class Price
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
