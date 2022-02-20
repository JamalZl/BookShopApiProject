using BookShopMvc.DTOs;
using BookShopMvc.DTOs.AuthorDtos;
using BookShopMvc.DTOs.BookDtos;
using BookShopMvc.DTOs.GenreDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookShopMvc.Controllers
{
    public class BookController : Controller
    {

        public async Task<IActionResult> Index()
        {
            BookListDto bookDto;

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44353/admin/api/books");
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    bookDto = JsonConvert.DeserializeObject<BookListDto>(responseStr);
                    return View(bookDto);
                }
            }
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Create()
        {
            if (!ModelState.IsValid) return View();

            ListDto<AuthorListItemDto> authorDto;
            ListDto<GenreListItemDto> genreDto;
            var authorEndpoint = "https://localhost:44353/admin/api/authors";
            var genreEndpoint = "https://localhost:44353/admin/api/genres";


            using (HttpClient client = new HttpClient())
            {

                var authorResponse = await client.GetAsync(authorEndpoint);
                var authorResponseStr = await authorResponse.Content.ReadAsStringAsync();

                var genreResponse = await client.GetAsync(genreEndpoint);
                var genreResponseStr = await genreResponse.Content.ReadAsStringAsync();

                if (genreResponse.StatusCode == System.Net.HttpStatusCode.OK && authorResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    authorDto = JsonConvert.DeserializeObject<ListDto<AuthorListItemDto>>(authorResponseStr);
                    genreDto = JsonConvert.DeserializeObject<ListDto<GenreListItemDto>>(genreResponseStr);

                    BookPostGeneralDto bookgnDto = new BookPostGeneralDto
                    {
                        Authors = authorDto,
                        Genres = genreDto
                    };
                    return View(bookgnDto);
                }
                else
                {
                    return Json(authorResponse.StatusCode);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookPostGeneralDto bookGnDto)
        {
            if (!ModelState.IsValid) return View();
            using (HttpClient client = new HttpClient())
            {
                byte[] byteArr = null;

                if (bookGnDto.Books.ImageFile != null)
                {
                    using (var mStream = new MemoryStream())
                    {
                        bookGnDto.Books.ImageFile.CopyTo(mStream);
                        byteArr = mStream.ToArray();
                    }
                }
                else
                {
                    return BadRequest();
                }
                var byteArrContent = new ByteArrayContent(byteArr);
                byteArrContent.Headers.ContentType = MediaTypeHeaderValue.Parse(bookGnDto.Books.ImageFile.ContentType);
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(byteArrContent, "ImageFile", bookGnDto.Books.ImageFile.FileName);
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.Name), Encoding.UTF8, "application/json"), "Name");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.Language), Encoding.UTF8, "application/json"), "language");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.CostPrice), Encoding.UTF8, "application/json"), "costprice");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.SalePrice), Encoding.UTF8, "application/json"), "saleprice");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.PageCount), Encoding.UTF8, "application/json"), "pagecount");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.GenreId), Encoding.UTF8, "application/json"), "genreid");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.AuthorId), Encoding.UTF8, "application/json"), "authorid");

                string endpoint = "https://localhost:44353/admin/api/books";
                using (var Response = await client.PostAsync(endpoint, multipartContent))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("index", "book");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            if (!ModelState.IsValid) return View();

            ListDto<AuthorListItemDto> authorDto;
            ListDto<GenreListItemDto> genreDto;
            BookPostDto bookDto;
            var authorEndpoint = "https://localhost:44353/admin/api/authors";
            var genreEndpoint = "https://localhost:44353/admin/api/genres";
            var bookEndpoint = "https://localhost:44353/admin/api/books/"+id;


            using (HttpClient client = new HttpClient())
            {

                var authorResponse = await client.GetAsync(authorEndpoint);
                var authorResponseStr = await authorResponse.Content.ReadAsStringAsync();

                var bookResponse = await client.GetAsync(bookEndpoint);
                var bookResponseStr = await bookResponse.Content.ReadAsStringAsync();


                var genreResponse = await client.GetAsync(genreEndpoint);
                var genreResponseStr = await genreResponse.Content.ReadAsStringAsync();

                if (genreResponse.StatusCode == System.Net.HttpStatusCode.OK && authorResponse.StatusCode == System.Net.HttpStatusCode.OK && bookResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    authorDto = JsonConvert.DeserializeObject<ListDto<AuthorListItemDto>>(authorResponseStr);
                    genreDto = JsonConvert.DeserializeObject<ListDto<GenreListItemDto>>(genreResponseStr);
                    bookDto = JsonConvert.DeserializeObject<BookPostDto>(bookResponseStr);

                    BookPostGeneralDto bookgnDto = new BookPostGeneralDto
                    {
                        Authors = authorDto,
                        Genres = genreDto,
                        Books=bookDto
                       
                    };
                    return View(bookgnDto);
                }
                else
                {
                    return Json(bookResponse.StatusCode);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,BookPostGeneralDto bookGnDto)
        {
            if (!ModelState.IsValid) return View();
            using (HttpClient client = new HttpClient())
            {
                byte[] byteArr = null;

                if (bookGnDto.Books.ImageFile != null)
                {
                    using (var mStream = new MemoryStream())
                    {
                        bookGnDto.Books.ImageFile.CopyTo(mStream);
                        byteArr = mStream.ToArray();
                    }
                }
                else
                {
                    return BadRequest();
                }
                var byteArrContent = new ByteArrayContent(byteArr);
                byteArrContent.Headers.ContentType = MediaTypeHeaderValue.Parse(bookGnDto.Books.ImageFile.ContentType);
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(byteArrContent, "ImageFile", bookGnDto.Books.ImageFile.FileName);
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.Name), Encoding.UTF8, "application/json"), "Name");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.Language), Encoding.UTF8, "application/json"), "language");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.CostPrice), Encoding.UTF8, "application/json"), "costprice");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.SalePrice), Encoding.UTF8, "application/json"), "saleprice");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.PageCount), Encoding.UTF8, "application/json"), "pagecount");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.GenreId), Encoding.UTF8, "application/json"), "genreid");
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(bookGnDto.Books.AuthorId), Encoding.UTF8, "application/json"), "authorid");

                string endpoint = "https://localhost:44353/admin/api/books/"+id;
                using (var Response = await client.PutAsync(endpoint, multipartContent))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("index", "book");
                    }
                    else
                    {
                        return Json(Response.StatusCode);
                    }
                }
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync("https://localhost:44353/admin/api/books/" + id);
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("index", "book");
                }
            }
            return RedirectToAction("index", "book");
        }
    }
}
