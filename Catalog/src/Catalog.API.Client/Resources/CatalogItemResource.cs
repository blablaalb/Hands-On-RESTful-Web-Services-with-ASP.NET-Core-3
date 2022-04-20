using Catalog.API.Client.Base;
using Catalog.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Client.Resources
{
    public class CatalogItemResource : ICatalogItemResource
    {
        private readonly IBaseClient _client;

        public CatalogItemResource(IBaseClient client)
        {
            _client = client;
        }

        public async Task<ItemResponse> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var uri = BuildUri(id);
            return await _client.GetAsync<ItemResponse>(uri, cancellationToken);
        }

        private Uri BuildUri(Guid id, string path = "")
        {
            return _client.BuildUri(string.Format("api/items/{0}", id, path));
        }
    }
}
