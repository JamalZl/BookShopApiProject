using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Apps.AdminApi.DTOs.GenreDtos
{
    public class GenrePostDto
    {
        public string Name { get; set; }
    }

    public class GenrePostDtoValidator : AbstractValidator<GenrePostDto>
    {
        public GenrePostDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Name field can not be longer than 20 characters!").NotEmpty().WithMessage("Name field can not be empty");
        }
    }
}
