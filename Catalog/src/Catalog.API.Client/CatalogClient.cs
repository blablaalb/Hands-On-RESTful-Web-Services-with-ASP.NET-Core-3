using Catalog.API.Client.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using Catalog.API.Client.Base;

namespace Catalog.API.Client
{
    public class CatalogClient : ICatalogClient
    {
        public ICatalogItemResource Item { get; }

        public CatalogClient(HttpClient client)
        {
            Item = new CatalogItemResource(new BaseClient(client, client.BaseAddress.ToString()));
        }

    }
}
