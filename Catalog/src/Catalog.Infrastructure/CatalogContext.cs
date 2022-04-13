using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Domain.Repositories;
using System.Threading.Tasks;
using System.Threading;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.SchemaDefinitions;

namespace Catalog.Infrastructure
{
    public class CatalogContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "catalog";
        public DbSet<Item> Items { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Gengres { get; set; }

        public CatalogContext(DbContextOptions<CatalogContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemEntitySchemaDefinition());
            modelBuilder.ApplyConfiguration(new ArtistEntitySchemaConfiguration());
            modelBuilder.ApplyConfiguration(new GenreEntitySchemaConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
