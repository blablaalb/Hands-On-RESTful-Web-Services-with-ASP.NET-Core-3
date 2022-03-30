﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Tests.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder Seed<T>(this ModelBuilder modelBuilder, string file) where T: class
        {
            using (var reader = new StreamReader(file))
            {
                var json = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<T[]>(json);
                modelBuilder.Entity<T>().HasData(data);
            }
            return modelBuilder;
        }
    }
}
