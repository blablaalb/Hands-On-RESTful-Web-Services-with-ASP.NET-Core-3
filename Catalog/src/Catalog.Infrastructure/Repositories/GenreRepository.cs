using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly CatalogContext _catalogContext;

        public IUnitOfWork UnitOfWork => _catalogContext;

        public GenreRepository(CatalogContext catalogContext)
        {
            this._catalogContext = catalogContext;
        }

        public Genre Add(Genre genre)
        {
            return _catalogContext.Gengres.Add(genre).Entity;
        }

        public async Task<IEnumerable<Genre>> GetAsync()
        {
            var genres = await _catalogContext.Gengres.AsNoTracking().ToListAsync();
            return genres;
        }

        public async Task<Genre> GetAsync(Guid id)
        {
            var genre = await _catalogContext.Gengres.FindAsync(id);

            if (genre == null) return null;

            _catalogContext.Entry(genre).State = EntityState.Detached;

            return genre;
        }
    }
}
