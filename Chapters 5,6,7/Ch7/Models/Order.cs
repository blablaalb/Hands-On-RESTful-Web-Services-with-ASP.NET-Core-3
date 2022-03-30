using System;
using System.Collections.Generic;

namespace Ch7.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public IEnumerable<string> ItemsIds { get; set; }
        public string Currency { get; set; }
        public bool IsInactive { get; set; }
    }
}
