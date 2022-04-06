using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Xunit;
using System.Threading.Tasks;
using Catalog.Infrastructure.Repositories;
using Catalog.Domain.Entities;
using System.Linq;
using Catalog.Fixtures;
using Xunit.Abstractions;

namespace Catalog.Infrastructure.Tests
{
    public class ItemRepositoryTests : IClassFixture<CatalogContextFactory>
    {
        private readonly ItemRepository _sut; //system under test
        private readonly TestCatalogContext _context;

        public ItemRepositoryTests(CatalogContextFactory catalogContextFactory)
        {
            _context = catalogContextFactory.ContextInstance;
            _sut = new ItemRepository(_context);
        }

        [Fact]
        public async Task Should_Get_Data()
        {
            var result = await _sut.GetAsync();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Returns_Null_With_Id_Not_Present()
        {
            var result = await _sut.GetAsync(Guid.NewGuid());
            result.ShouldBeNull();
        }

        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task Should_Return_Record_By_Id(string guid)
        {
            var result = await _sut.GetAsync(new Guid(guid));
            result.Id.ShouldBe(new Guid(guid));
        }

        [Fact]
        public async Task Should_Add_New_Item()
        {
            var testItem = new Item
            {
                Name = "Test album",
                Description = "Description",
                LabelName = "Label Name",
                Price = new Price { Amount = 13, Currency = "EUR" },
                PictureUri = "https://mycdn.com/pictures/32423423",
                ReleaseDate = DateTimeOffset.Now,
                AvailableStock = 6,
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")
            };

            _sut.Add(testItem);
            await _sut.UnitOfWork.SaveEntitiesAsync();

            _context.Items.FirstOrDefault(_ => _.Id == testItem.Id).ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Update_Item()
        {
            var testItem = new Item
            {
                Id = new Guid("b5b05534-9263-448c-a69e-0bbd8b3eb90e"),
                Name = "Test album",
                Description = "Description updated",
                LabelName = "Label name",
                Price = new Price { Amount = 50, Currency = "EUR" },
                PictureUri = "https://mycdn.com/pictures/32423423",
                ReleaseDate = DateTimeOffset.Now,
                AvailableStock = 6,
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")
            };

            _sut.Update(testItem);
            await _sut.UnitOfWork.SaveEntitiesAsync();
            var result = _context.Items.FirstOrDefault(item => item.Id == testItem.Id);

            result.Description.ShouldBe("Description updated");
            result.Price.Amount.ShouldBe(50);
        }

    }
}
