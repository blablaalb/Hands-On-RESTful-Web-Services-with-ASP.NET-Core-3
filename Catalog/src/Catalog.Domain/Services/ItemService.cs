using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using Catalog.Domain.Configurations;
using Catalog.Domain.Events;
using Newtonsoft.Json;

namespace Catalog.Domain.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemMapper _itemMapper;
        private readonly ConnectionFactory _eventBusConnectionFactory;
        private readonly ILogger<ItemService> _logger;
        private readonly EventBusSettings _settings;

        public ItemService(IItemRepository itemRepository, IItemMapper itemMapper, ConnectionFactory eventBusConnectionFactory, ILogger<ItemService> logger, EventBusSettings settings)
        {
            this._itemRepository = itemRepository;
            this._itemMapper = itemMapper;
            this._eventBusConnectionFactory = eventBusConnectionFactory;
            this._logger = logger;
            this._settings = settings;
        }

        public async Task<ItemResponse> AddItemAsync(AddItemRequest request)
        {
            var item = _itemMapper.Map(request);
            var result = _itemRepository.Add(item);

            await _itemRepository.UnitOfWork.SaveChangesAsync();

            return _itemMapper.Map(result);
        }

        public async Task<ItemResponse> DeleteItemAsync(DeleteItemRequest request)
        {
            if (request?.Id == null) throw new ArgumentNullException();

            var result = await _itemRepository.GetAsync(request.Id);
            result.IsInactive = true;

            _itemRepository.Update(result);
            await _itemRepository.UnitOfWork.SaveChangesAsync();

            return _itemMapper.Map(result);
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

        private void SendDeleteMessage(ItemSoldOutEvent message)
        {
            try
            {
                var connection = _eventBusConnectionFactory.CreateConnection();

                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: _settings.EventQueue, true, false);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: _settings.EventQueue, body: body);
                channel.WaitForConfirmsOrDie();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Unable to initialize the event bus: {message}", ex.Message);
            }
        }
    }
}
