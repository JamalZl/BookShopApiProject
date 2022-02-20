using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.DTOs.GenreDtos
{
    public class GenreListDto
    {
        public List<GenreListItemDto> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
