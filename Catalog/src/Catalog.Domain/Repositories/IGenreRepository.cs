using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.Repositories
{
    public interface IGenreRepository
    {
         IUnitOfWork UnitOfWork { get; }
        Task<IEnumerable<Genre>> GetAsync();
        Task<Genre> GetAsync(Guid id);
        Genre Add(Genre genre);
    }
}
