﻿using Catalog.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Fixtures
{
    public class InMemoryWebApplicationFactory<TStartup>: WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Testing")
                .ConfigureTestServices(services =>
                {
                    var options = new DbContextOptionsBuilder<CatalogContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

                    services.AddScoped<CatalogContext>(serviceProvider => new TestCatalogContext(options));
                    services.Replace(ServiceDescriptor.Scoped(_ => new UserContextFactory().InMemoryUserManager));
                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CatalogContext>();
                    db.Database.EnsureCreated();
                });
        }
    }
}
