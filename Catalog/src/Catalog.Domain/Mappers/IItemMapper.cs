using Catalog.Domain.Entities;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Mappers
{
    public interface IItemMapper
    {
        Item Map(AddItemRequest request);
        Item Map(EditItemRequest request);
        ItemResponse Map(Item item);
    }
}
