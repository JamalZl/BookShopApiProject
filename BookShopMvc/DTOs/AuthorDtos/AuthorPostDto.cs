using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.DTOs.AuthorDtos
{
    public class AuthorPostDto
    {
        public int Id { get; set; }
        [StringLength(maximumLength:20)]
        [Required]
        public string Name { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Image { get; set; }
    }
}
