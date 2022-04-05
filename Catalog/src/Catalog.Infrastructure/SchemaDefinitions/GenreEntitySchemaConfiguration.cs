﻿using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.SchemaDefinitions
{
    public class GenreEntitySchemaConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genres", CatalogContext.DEFAULT_SCHEMA);
            builder.HasKey(k => k.GenreId);
            builder.Property(p => p.GenreDescription);

            builder.Property(p => p.GenreDescription).IsRequired().HasMaxLength(1000);
        }
    }
}
