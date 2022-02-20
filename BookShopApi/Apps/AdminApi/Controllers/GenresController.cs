using AutoMapper;
using BookShopApi.Apps.AdminApi.DTOs;
using BookShopApi.Apps.AdminApi.DTOs.GenreDtos;
using BookShopApi.Data.DAL;
using BookShopApi.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Apps.AdminApi.Controllers
{
    [ApiExplorerSettings(GroupName = "admin_v1")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly BookShopDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        public GenresController(BookShopDbContext context, IWebHostEnvironment env,IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        [HttpPost("")]
        public IActionResult Create(GenrePostDto genreDto)
        {
            if (_context.Genres.Any(a => a.Name.ToLower().Trim() == genreDto.Name.ToLower().Trim()))
                return StatusCode(409);
            Genre genre = new Genre
            {
                Name = genreDto.Name
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return StatusCode(201, genre);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Genre genre = _context.Genres.Include(a => a.Books).FirstOrDefault(a => a.Id == id && !a.IsDeleted);

            if (genre == null)
                return NotFound();

            GenreGetDto genreGetDto = _mapper.Map<GenreGetDto>(genre);

            return Ok(genreGetDto);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var query = _context.Genres.Where(a => !a.IsDeleted);

            ListDto<GenreListItemDto> listDto = new ListDto<GenreListItemDto>
            {
                Items = query.Select(a => new GenreListItemDto { Id = a.Id, Name = a.Name }).ToList(),
                TotalCount = query.Count()
            };
            return Ok(listDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id,GenrePostDto genreDto)
        {
            Genre genre = _context.Genres.FirstOrDefault(a => a.Id == id && !a.IsDeleted);

            if (genre == null)
                return NotFound();
            if (_context.Genres.Any(a => a.Id != id && a.Name.ToLower().Trim() == genreDto.Name.ToLower().Trim()))
                return StatusCode(409);
            genre.Name = genreDto.Name;
            genre.ModifiedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Genre genre = _context.Genres.FirstOrDefault(a => a.Id == id && !a.IsDeleted);
            if (genre == null)
                return NotFound();

            genre.IsDeleted = true;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
