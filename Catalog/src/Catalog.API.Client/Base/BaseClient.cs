using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Catalog.API.Client.Base
{
    public class BaseClient : IBaseClient
    {
        private readonly HttpClient _client;
        private readonly string _baseUri;

        public BaseClient(HttpClient httpClient, string baseUri)
        {
            _client = httpClient;
            _baseUri = baseUri;
        }


        public async Task<T> GetAsync<T>(Uri uri, CancellationToken cancellationToken)
        {
            var result = await _client.GetAsync(uri, cancellationToken);
            result.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }

        public Uri BuildUri(string format)
        {
            return new UriBuilder(_baseUri)
            {
                Path = format
            }.Uri;
        }

    }


}
