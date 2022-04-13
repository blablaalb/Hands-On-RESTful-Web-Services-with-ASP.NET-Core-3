using Catalog.Domain.Entities;
using Catalog.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Catalog.Infrastructure.Repositories;
using Shouldly;
using System.Linq;

namespace Catalog.Infrastructure.Tests
{
    public class ArtistRepositoryTests: IClassFixture<CatalogContextFactory>
    {
        private readonly CatalogContextFactory _factory;

        public ArtistRepositoryTests(CatalogContextFactory factory)
        {
                _factory= factory;
        }

        [Theory]
        [LoadData("artist")]
        public async Task Should_Return_Record_By_Id(Artist artist)
        {
            var sut = new ArtistRepository(_factory.ContextInstance);
            var result = await sut.GetAsync(artist.ArtistId);
            result.ArtistName.ShouldBe(artist.ArtistName);
        }

        [Theory]
        [LoadData("artist")]
        public async Task Should_Add_New_Item(Artist artist)
        {
            artist.ArtistId = Guid.NewGuid();
            var sut = new ArtistRepository(_factory.ContextInstance);
            sut.Add(artist);
            await sut.UnitOfWork.SaveEntitiesAsync();

            _factory.ContextInstance.Artists.FirstOrDefault(x => x.ArtistId == artist.ArtistId).ShouldNotBeNull();
        }
    }
}
