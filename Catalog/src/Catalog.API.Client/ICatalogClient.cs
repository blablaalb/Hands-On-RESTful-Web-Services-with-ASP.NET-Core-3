using Catalog.API.Client.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.API.Client
{
    public interface ICatalogClient
    {
        ICatalogItemResource Item { get; }
    }
}
