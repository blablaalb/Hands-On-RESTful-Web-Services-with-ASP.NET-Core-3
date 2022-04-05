using Catalog.Domain.Entities;
using Catalog.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Mappers
{
    public class ArtistMapper : IArtistMapper
    {
        public ArtistResponse Map(Artist artist)
        {
            if (artist == null) return null;

            var response = new ArtistResponse
            {
                ArtistId = artist.ArtistId,
                ArtistName = artist.ArtistName
            };
            return response;
        }
    }
}
