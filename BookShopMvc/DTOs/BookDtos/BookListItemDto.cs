using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.DTOs.BookDtos
{
    public class BookListItemDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        [Required]
        public bool DisplayStatus { get; set; }
        [Required]
        public decimal CostPrice { get; set; }
        [Required]
        public decimal SalePrice { get; set; }
        [Required]
        public int PageCount { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Language { get; set; }
        public GenreInBookListItemDto Genre { get; set; }
        public AuthorInBookListItemDto Author { get; set; }
    }

    public class GenreInBookListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AuthorInBookListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
