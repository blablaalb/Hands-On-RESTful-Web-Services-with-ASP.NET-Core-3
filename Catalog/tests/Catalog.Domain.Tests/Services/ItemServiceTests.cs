using Catalog.Domain.Entities;
using Catalog.Domain.Mappers;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Services;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Shouldly;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.Domain.Tests.Services
{
    public class ItemServiceTests : IClassFixture<CatalogContextFactory>
    {
        private readonly ItemRepository _itemRepository;
        private readonly IItemMapper _mapper;

        public ItemServiceTests(CatalogContextFactory catalogContextFactory)
        {
            _itemRepository = new ItemRepository(catalogContextFactory.ContextInstance);
            _mapper = catalogContextFactory.ItemMapper;
        }

        [Fact]
        public async Task AddItem_Should_Add_Right_Entity()
        {
            var testItem = new AddItemRequest
            {
                Name = "Test album",
                Description = "Description",
                LabelName = "Label name",
                Price = new Price { Amount = 13, Currency = "EUR" },
                PictureUri = "https://mycdn.com/pictures/32423423",
                ReleaseDate = DateTimeOffset.Now,
                Format = "Vinyl 33g",
                AvailableStock = 6,
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")
            };

            IItemService sut = new ItemService(_itemRepository, _mapper);

            var result = await sut.AddItemAsync(testItem);

            result.Name.ShouldBe(testItem.Name);
            result.Description.ShouldBe(testItem.Description);
            result.LabelName.ShouldBe(testItem.LabelName);
            result.GenreId.ShouldBe(testItem.GenreId);
            result.ArtistId.ShouldBe(testItem.ArtistId);
            result.Price.Amount.ShouldBe(testItem.Price.Amount);
            result.Price.Currency.ShouldBe(testItem.Price.Currency);
        }

        [Fact]
        public async Task EditItem_Should_Add_Right_Entity()
        {
            var testItem = new EditItemRequest
            {
                Id = new Guid("b5b05534-9263-448c-a69e-0bbd8b3eb90e"),
                Name = "Test album",
                Description = "Description",
                LabelName = "Label name",
                Price = new Price { Amount = 13, Currency = "EUR" },
                PictureUri = "https://mycdn.com/pictures/32423423",
                ReleaseDate = DateTimeOffset.Now,
                Format = "Vinyl 33g",
                AvailableStock = 6,
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")
            };

            ItemService sut = new ItemService(_itemRepository, _mapper);
            var result = await sut.EditItemAsync(testItem);

            result.Name.ShouldBe(testItem.Name);
            result.Description.ShouldBe(testItem.Description);
            result.GenreId.ShouldBe(testItem.GenreId);
            result.ArtistId.ShouldBe(testItem.ArtistId);
            result.Price.Amount.ShouldBe(testItem.Price.Amount);
            result.Price.Currency.ShouldBe(testItem.Price.Currency);
        }

        [Fact]
        public async Task GetItems_Should_Return_Right_Data()
        {
            ItemService sut = new ItemService(_itemRepository, _mapper);

            var result = await sut.GetItemsAsync();

            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task GetItem_Should_Return_Right_Data(string guid)
        {
            ItemService sut = new ItemService(_itemRepository, _mapper);

            var result = await sut.GetItemAsync(new GetItemRequest { Id = new Guid(guid) });
            result.Id.ShouldBe(new Guid(guid));
        }

        [Fact]
        public void GetItem_Should_Throw_Exception_With_Null_Id()
        {
            ItemService sut = new ItemService(_itemRepository, _mapper);
            sut.GetItemAsync(null).ShouldThrow<ArgumentNullException>();
        }




    }
}
