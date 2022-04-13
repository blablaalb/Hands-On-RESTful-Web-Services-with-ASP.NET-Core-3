﻿using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly CatalogContext _catalogContext;

        public IUnitOfWork UnitOfWork => _catalogContext;

        public ArtistRepository(CatalogContext catalogContext)
        {
            this._catalogContext = catalogContext;
        }

        public Artist Add(Artist artist)
        {
            return _catalogContext.Artists.Add(artist).Entity;
        }

        public async Task<IEnumerable<Artist>> GetAsync()
        {
            var artists = await _catalogContext.Artists.AsNoTracking().ToListAsync();
            return artists;
        }

        public async Task<Artist> GetAsync(Guid id)
        {
            var artist = await _catalogContext.Artists.FindAsync(id);

            if (artist == null) return null;

            _catalogContext.Entry(artist).State = EntityState.Detached;

            return artist;
        }
    }
}
