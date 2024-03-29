﻿using Catalog.API.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Catalog.Infrastructure;
using Microsoft.Extensions.Hosting;
using Catalog.Domain.Repositories;
using Catalog.Domain.Extensions;
using Catalog.Infrastructure.Repositories;
using RiskFirst.Hateoas;
using Catalog.API.Responses;
using Catalog.API.Controllers;
using Catalog.API.Middleware;
using Polly;
using Catalog.Infrastructure.Extensions;
using Catalog.Domain.Services;

namespace Catalog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCatalogContext(Configuration.GetSection("DataSource:ConnectionString").Value)
                .AddScoped<IItemRepository, ItemRepository>()
                .AddScoped<IArtistRepository, ArtistRepository>()
                .AddScoped<IGenreRepository, GenreRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IUserService, UserService>()
                .AddMappers()
                .AddServices()
                .AddControllers()
                .AddValidation();
            services.AddEventBus(Configuration);
            services.AddTokenAuthentication(Configuration);

            services.AddLinks(config =>
               config.AddPolicy<ItemHateoasResponse>(policy =>
               {
                   policy
                   .RequireRoutedLink(nameof(ItemsHateoasController.Get), nameof(ItemsHateoasController.Get))
                   .RequireRoutedLink(nameof(ItemsHateoasController.GetById), nameof(ItemsHateoasController.GetById), _ => new { id = _.Data.Id })
                   .RequireRoutedLink(nameof(ItemsHateoasController.Post), nameof(ItemsHateoasController.Post))
                   .RequireRoutedLink(nameof(ItemsHateoasController.Put), nameof(ItemsHateoasController.Put), x => new { id = x.Data.Id })
                   .RequireRoutedLink(nameof(ItemsHateoasController.Delete), nameof(ItemsHateoasController.Delete), x => new { id = x.Data.Id });
               }
               ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ResponseTimeMiddlewareAsync>();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseCors(cfg =>
            {
                cfg.AllowAnyOrigin();
            });
        }
    }
}
