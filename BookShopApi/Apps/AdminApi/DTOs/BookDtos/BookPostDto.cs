using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Apps.AdminApi.DTOs.BookDtos
{
    public class BookPostDto
    {
        public string Name { get; set; }
        public bool DisplayStatus { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public IFormFile Image { get; set; }
        public int PageCount { get; set; }
        public string Language { get; set; }
        public int GenreId { get; set; }
        public int AuthorId { get; set; }
        
    }
    public class BookPostDtoValidator : AbstractValidator<BookPostDto>
    {
        public BookPostDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(50).WithMessage("Name field can not be longer than 50 characters!")
                .NotEmpty().WithMessage("Name mecburidir!");

            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0).WithMessage("CostPrice can not be less than 0")
                .NotEmpty().WithMessage("CostPrice mecburidir!");

            RuleFor(x => x.SalePrice)
                .GreaterThanOrEqualTo(0).WithMessage("SalePrice can not be less than 0")
                .NotEmpty().WithMessage("SalePrice mecburidir!");

            RuleFor(x => x.DisplayStatus).NotEmpty().WithMessage("DisplayStatus is required!");

            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.CostPrice > x.SalePrice)
                    context.AddFailure("CostPrice", "CostPrice can not be more than SalePrice");
            });
            RuleFor(x => x.Language).MaximumLength(50).WithMessage("Name field can not be longer than 50 characters!").NotEmpty().WithMessage("Can not be empty");
            RuleFor(x => x.PageCount).NotEmpty().WithMessage("Can not be empty");
        }
    }
}
