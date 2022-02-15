using BookShopApi.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Apps.AdminApi.DTOs
{
    public class AuthorPostDto
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }

    }

    public class AuthorPostDtoValidator : AbstractValidator<AuthorPostDto>
    {
        public AuthorPostDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Name field can not be longer than 50 characters!").NotEmpty().WithMessage("Name field can not be empty");
            RuleFor(x => x.Image).Custom((x, content) =>
            {
                if (x==null)
                {                                                 
                    content.AddFailure("Image", "Image can not be null");
                }
                else if(!x.IsImage())
                {
                    content.AddFailure("Image", "Please insert a valid image type such as jpg,png,jpeg etc");
                }
               else if(!x.IsSizeOkay(2))
                {
                    content.AddFailure("Image", "Image size can not be more than 2MB");
                }
            });
        }
        
    }
}
