using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ch7.Requests
{
    public class OrderRequest
    {
        [Required]
        public IEnumerable<string> ItemsIds { get; set; }
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }
    }
}
