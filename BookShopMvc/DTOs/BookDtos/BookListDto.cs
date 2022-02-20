using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.DTOs.BookDtos
{
    public class BookListDto
    {
        public List<BookListItemDto> Items { get; set; }
    }
}
