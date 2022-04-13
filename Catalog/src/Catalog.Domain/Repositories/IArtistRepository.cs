using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.Repositories
{
    public interface IArtistRepository
    {
        IUnitOfWork UnitOfWork { get; }
        Task<IEnumerable<Artist>> GetAsync();
        Task <Artist> GetAsync(Guid id);
        Artist Add(Artist artist);
    }
}
