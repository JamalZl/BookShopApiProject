using BookShopMvc.DTOs.AuthorDtos;
using BookShopMvc.DTOs.GenreDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.DTOs.BookDtos
{
    public class BookPostGeneralDto
    {
        public ListDto<GenreListItemDto> Genres { get; set; }
        public ListDto<AuthorListItemDto> Authors { get; set; }
        public BookPostDto Books { get; set; }
    }
}
