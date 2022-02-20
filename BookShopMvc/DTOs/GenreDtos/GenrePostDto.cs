using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.DTOs.GenreDtos
{
    public class GenrePostDto
    {

        public int Id { get; set; }
        [StringLength(maximumLength:20)]
        [Required]
        public string Name { get; set; }
    }
}
