﻿using Catalog.Domain.Mappers;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Catalog.Infrastructure;

namespace Catalog.Fixtures
{
    public class CatalogContextFactory
    {
        public readonly TestCatalogContext ContextInstance;
        public readonly IGenreMapper GenreMapper;
        public readonly IArtistMapper ArtistMapper;
        public readonly IItemMapper ItemMapper;

        public CatalogContextFactory()
        {
            var contextOptions = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            EnsureCreation(contextOptions);
            ContextInstance = new TestCatalogContext(contextOptions);
            GenreMapper = new GenreMapper();
            ArtistMapper = new ArtistMapper();
            ItemMapper = new ItemMapper(ArtistMapper, GenreMapper);
        }

        private void EnsureCreation(DbContextOptions<CatalogContext> contextOptions)
        {
            using var context = new TestCatalogContext(contextOptions);
            context.Database.EnsureCreated();
        }
    }
}
