using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Services
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistResponse>> GetArtistsAsync();
        Task<ArtistResponse> GetArtistAsync(GetArtistRequest request);
        Task<IEnumerable<ItemResponse>> GetItemByArtistIdAsync(GetArtistRequest request);
        Task<ArtistResponse> AddArtistAsync(AddArtistRequest request);
    }
}
