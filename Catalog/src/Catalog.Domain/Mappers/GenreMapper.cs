using Catalog.Domain.Entities;
using Catalog.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Mappers
{
    public class GenreMapper : IGenreMapper
    {
        public GenreResponse Map(Genre genre)
        {
            if (genre == null) return null;

            var response = new GenreResponse
            {
                GenreDescription = genre.GenreDescription,
                GenreId = genre.GenreId
            };
            return response;
        }
    }
}
