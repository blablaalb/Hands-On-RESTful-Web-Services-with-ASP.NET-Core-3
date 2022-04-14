using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Requests.Artists.Validators
{
    public class AddArtistRequestValidator: AbstractValidator<AddArtistRequest>
    {
        public AddArtistRequestValidator()
        {
            RuleFor(artist => artist.ArtistName).NotEmpty();
        }
    }
}
