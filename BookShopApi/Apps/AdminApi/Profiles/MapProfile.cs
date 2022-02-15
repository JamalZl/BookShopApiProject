using AutoMapper;
using BookShopApi.Apps.AdminApi.DTOs;
using BookShopApi.Apps.AdminApi.DTOs.BookDtos;
using BookShopApi.Apps.AdminApi.DTOs.GenreDtos;
using BookShopApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Apps.AdminApi.Profiles
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Author, AuthorGetDto>();
            CreateMap<BookPostDto, Book>();
            CreateMap<Genre, GenreGetDto>();
            CreateMap<Genre, GenreInBookDto>();
            CreateMap<Author, AuthorInBookDto>();
            CreateMap<Book, BookGetDto>()
                .ForMember(dest => dest.Profit, map => map.MapFrom(src => src.SalePrice - src.CostPrice));
        }
    }
}
