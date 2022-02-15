using AutoMapper;
using BookShopApi.Apps.AdminApi.DTOs;
using BookShopApi.Apps.AdminApi.DTOs.BookDtos;
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
    [Route("admin/api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookShopDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        public BooksController(BookShopDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        [HttpPost("")]
        public IActionResult Create([FromForm] BookPostDto bookDto)
        {
            if (_context.Books.Any(a => a.Name.ToLower().Trim() == bookDto.Name.ToLower().Trim()) && _context.Books.Any(b=>b.Language.ToLower().Trim()==bookDto.Language.ToLower().Trim()))
                return StatusCode(409);

            Book book = new Book
            {
                Name = bookDto.Name,
                DisplayStatus = bookDto.DisplayStatus,
                CostPrice = bookDto.CostPrice,
                SalePrice = bookDto.SalePrice,
                PageCount = bookDto.PageCount,
                Language = bookDto.Language,
                GenreId = bookDto.GenreId,
                AuthorId = bookDto.AuthorId
            };
            
            book.Image = bookDto.Image.SaveImg(_env.WebRootPath, "assets/uploads/image");

            _context.Books.Add(book);
            _context.SaveChanges();
            return StatusCode(201, book);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Book book = _context.Books.Include(x => x.Genre).Include(b=>b.Author).FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (book == null) return NotFound();

            BookGetDto bookDto = _mapper.Map<BookGetDto>(book);

            return Ok(bookDto);
        }
        [HttpGet]
        public IActionResult GetAll()
        {

            var query = _context.Books.Include(p => p.Genre).Include(b=>b.Author).Where(p => !p.IsDeleted);
            ListDto<BookListItemDto> listDto = new ListDto<BookListItemDto>
            {
                Items = query.Select(p => new BookListItemDto
                {
                    Name = p.Name,
                    Language=p.Language,
                    PageCount=p.PageCount,
                    SalePrice = p.SalePrice,
                    CostPrice = p.CostPrice,
                    DisplayStatus = p.DisplayStatus,
                    Profit=p.SalePrice-p.CostPrice,
                    Genre = new GenreInBookListItemDto
                    {
                        Id = p.GenreId,
                        Name = p.Genre.Name
                    },
                    Author=new AuthorInBookListItemDto
                    {
                        Id=p.AuthorId,
                        Name=p.Author.Name
                    }

                }).ToList(),
                TotalCount = query.Count()
            };


            return StatusCode(201, listDto);
        }

        [HttpPut, Route("{id}")]
        public IActionResult Update(int id,[FromForm] BookPostDto bookDto)
        {
            Book existBook = _context.Books.FirstOrDefault(p => p.Id == id);
            if (existBook == null) return NotFound();

            if (existBook.GenreId != bookDto.GenreId && existBook.AuthorId!=bookDto.AuthorId && !_context.Books.Any(p => p.Id == bookDto.GenreId && p.Id==bookDto.AuthorId  && !p.IsDeleted))
                return NotFound();

            if (_context.Books.Any(a => a.Id != id && a.Name.ToLower().Trim() == bookDto.Name.ToLower().Trim()))
                return StatusCode(409);

            Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/uploads/image", existBook.Image);
            existBook.Image = bookDto.Image.SaveImg(_env.WebRootPath, "assets/uploads/image");
            existBook.CostPrice = bookDto.CostPrice;
            existBook.SalePrice = bookDto.SalePrice;
            existBook.Name = bookDto.Name;
            existBook.DisplayStatus = bookDto.DisplayStatus;
            existBook.GenreId = bookDto.GenreId;
            existBook.Language = bookDto.Language;
            existBook.PageCount = bookDto.PageCount;
            _context.SaveChanges();
            return NoContent();

        }

        [HttpDelete, Route("{id}")]
        public IActionResult Delete(int id)
        {
            Book book = _context.Books.FirstOrDefault(c => c.Id == id);
            if (book == null) return NotFound();

            book.IsDeleted = true;
            return NoContent();
        }

    }
}
