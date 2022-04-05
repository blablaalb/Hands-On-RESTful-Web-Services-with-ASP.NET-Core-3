using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests;
using Catalog.Domain.Responses;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemMapper _itemMapper;

        public ItemService(IItemRepository itemRepository, IItemMapper itemMapper)
        {
            this._itemRepository = itemRepository;
            this._itemMapper = itemMapper;
        }

        public async Task<ItemResponse> AddItemAsync(AddItemRequest request)
        {
            var item = _itemMapper.Map(request);
            var result = _itemRepository.Add(item);

            await _itemRepository.UnitOfWork.SaveChangesAsync();

            return _itemMapper.Map(result);
        }

        public Task<ItemResponse> DeleteItemAsync(DeleteItemRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemResponse> EditItemAsync(EditItemRequest request)
        {
            var existingRecord = await _itemRepository.GetAsync(request.Id);
            if (existingRecord == null) throw new ArgumentException($"Entity with id {request.Id} is not present");
            var entity = _itemMapper.Map(request);
            var result = _itemRepository.Update(entity);

            await _itemRepository.UnitOfWork.SaveChangesAsync();

            return _itemMapper.Map(result);
        }

        public async Task<ItemResponse> GetItemAsync(GetItemRequest request)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var entity = await _itemRepository.GetAsync(request.Id);
            return _itemMapper.Map(entity);
        }

        public async Task<IEnumerable<ItemResponse>> GetItemsAsync()
        {
            var result = await _itemRepository.GetAsync();
            return result.Select(x => _itemMapper.Map(x));
        }
    }
}
