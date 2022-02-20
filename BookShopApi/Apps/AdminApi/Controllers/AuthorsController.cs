using AutoMapper;
using BookShopApi.Apps.AdminApi.DTOs;
using BookShopApi.Data.DAL;
using BookShopApi.Data.Entities;
using BookShopApi.Extensions;
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
    public class AuthorsController : ControllerBase
    {
        private readonly BookShopDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public AuthorsController(BookShopDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        [HttpPost("")]
        public IActionResult Create([FromForm] AuthorPostDto authorDto)
        {
            if (_context.Authors.Any(a => a.Name.ToLower().Trim() == authorDto.Name.ToLower().Trim()))
                return StatusCode(409);


            Author author = new Author
            {
                Name = authorDto.Name
            };
            author.Image = authorDto.ImageFile.SaveImg(_env.WebRootPath, "assets/uploads/image");
            _context.Authors.Add(author);
            _context.SaveChanges();
            return StatusCode(201, author);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Author author = _context.Authors.Include(a => a.Books).FirstOrDefault(a => a.Id == id && !a.IsDeleted);

            if (author == null)
                return NotFound();

            AuthorGetDto authorGetDto = _mapper.Map<AuthorGetDto>(author);

            return Ok(authorGetDto);
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var query = _context.Authors.Where(a => !a.IsDeleted);

            ListDto<AuthorListItemDto> listDto = new ListDto<AuthorListItemDto>
            {
                Items = query.Select(a => new AuthorListItemDto { Id = a.Id, Name = a.Name }).ToList(),
                TotalCount = query.Count()
            };
            return Ok(listDto);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] AuthorPostDto authorDto)
        {
            Author author = _context.Authors.FirstOrDefault(a => a.Id == id && !a.IsDeleted);

            if (author == null)
                return NotFound();

            Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/uploads/image", author.Image);
            author.Image = authorDto.ImageFile.SaveImg(_env.WebRootPath, "assets/uploads/image");
            if (_context.Authors.Any(a => a.Id != id && a.Name.ToLower().Trim() == authorDto.Name.ToLower().Trim()))
                return StatusCode(409);
            author.Name = authorDto.Name;
            author.ModifiedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Author author = _context.Authors.FirstOrDefault(a => a.Id == id && !a.IsDeleted);
            if (author == null)
                return NotFound();

            author.IsDeleted = true;
            _context.SaveChanges();

            return NoContent();
        }
    }
}
