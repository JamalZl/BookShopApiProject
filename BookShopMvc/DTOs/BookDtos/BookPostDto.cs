using BookShopMvc.DTOs.AuthorDtos;
using BookShopMvc.DTOs.GenreDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.DTOs.BookDtos
{
    public class BookPostDto
    {
        [Required]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
        [Required]
        public bool DisplayStatus { get; set; }
        [Required]
        public decimal CostPrice { get; set; }
        [Required]
        public decimal SalePrice { get; set; }
        public IFormFile ImageFile { get; set; }
        [Required]
        public int PageCount { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        public string Language { get; set; }
        public int GenreId { get; set; }
        public int AuthorId { get; set; }
    }
}
