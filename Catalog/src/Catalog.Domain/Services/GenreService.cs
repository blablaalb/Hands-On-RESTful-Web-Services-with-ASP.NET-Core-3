﻿using Catalog.Domain.Entities;
using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Artists;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IGenreMapper _genreMapper;
        private readonly IItemMapper _itemMapper;

        public GenreService(IGenreRepository genreRepository, IItemRepository itemRepository, IGenreMapper genreMapper, IItemMapper itemMapper)
        {
            _genreRepository = genreRepository;
            _itemRepository = itemRepository;
            _genreMapper = genreMapper;
            _itemMapper = itemMapper;
        }

        public async Task<GenreResponse> AddGenreAsync(AddGenreRequest request)
        {
            var item = new Genre { GenreDescription = request.GenreDescription };
            var result = _genreRepository.Add(item);
            await _genreRepository.UnitOfWork.SaveChangesAsync();

            return _genreMapper.Map(result);
        }

        public async Task<GenreResponse> GetGenreAsync(GetGenreRequest request)
        {
            if (request?.Id == null) throw new ArgumentNullException();

            var result = await _genreRepository.GetAsync(request.Id);

            return result == null ? null : _genreMapper.Map(result);
        }

        public async Task<IEnumerable<GenreResponse>> GetGenreAsync()
        {
            var result = await _genreRepository.GetAsync();
            return result.Select(_genreMapper.Map);
        }

        public async Task<IEnumerable<ItemResponse>> GetItemByGenreIdAsync(GetGenreRequest request)
        {
            if (request?.Id == null) throw new ArgumentNullException();

            var result = await _itemRepository.GetItemByGenreIdAsync(request.Id);

            return result.Select(_itemMapper.Map);
        }
    }
}
