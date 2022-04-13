using Catalog.Domain.Entities;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Requests.Item.Validators;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Catalog.Domain.Tests.Requests.Item.Validators
{
    public class AddItemRequestValidatorTests
    {
        private readonly AddItemRequestValidator _validator;

        public AddItemRequestValidatorTests()
        {
            _validator = new AddItemRequestValidator();
        }

        [Fact]
        public void Should_Have_Error_When_ArtistId_Is_Null()
        {
            var addItemRequest = new AddItemRequest { Price = new Price() };
            _validator.ShouldHaveValidationErrorFor(x => x.ArtistId, addItemRequest);
        }

        [Fact]
        public void Should_Have_Error_When_GenreId_Is_Null()
        {
            var addItemRequest = new AddItemRequest { Price = new Price() };
            _validator.ShouldHaveValidationErrorFor(x => x.GenreId, addItemRequest);
        }
    }
}
