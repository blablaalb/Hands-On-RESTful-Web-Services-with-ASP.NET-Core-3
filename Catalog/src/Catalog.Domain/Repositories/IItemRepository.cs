using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Repositories
{
    public interface IItemRepository
    {
        IUnitOfWork UnitOfWork { get; }
        Task<IEnumerable<Item>> GetAsync();
        Task<Item> GetAsync(Guid id);
        Item Add(Item item);
        Item Update(Item item);
        Item Delete(Item item);
        Task<IEnumerable<Item>> GetItemByArtistIdAsync(Guid id);
        Task<IEnumerable<Item>> GetItemByGenreIdAsync (Guid id);
    }
}
