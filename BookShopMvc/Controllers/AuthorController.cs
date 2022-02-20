using BookShopMvc.DTOs.AuthorDtos;
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
    public class AuthorController : Controller
    {
        public async Task<IActionResult> Index()
        {
            AuthorListDto authorDto;

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44353/admin/api/authors");
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    authorDto = JsonConvert.DeserializeObject<AuthorListDto>(responseStr);
                    return View(authorDto);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorPostDto authorDto)
        {
            if (!ModelState.IsValid) return View();
            using (HttpClient client = new HttpClient())
            {
                byte[] byteArr = null;

                if (authorDto.ImageFile!=null)
                {
                    using (var mStream = new MemoryStream())
                    {
                        authorDto.ImageFile.CopyTo(mStream);
                        byteArr = mStream.ToArray();
                    }
                }
                else
                {
                    return BadRequest();
                }
                var byteArrContent = new ByteArrayContent(byteArr);
                byteArrContent.Headers.ContentType = MediaTypeHeaderValue.Parse(authorDto.ImageFile.ContentType);
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(authorDto.Name), Encoding.UTF8, "application/json"), "Name");
                multipartContent.Add(byteArrContent, "ImageFile", authorDto.ImageFile.FileName);

                string endpoint = "https://localhost:44353/admin/api/authors";
                using (var Response = await client.PostAsync(endpoint, multipartContent))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("index", "author");
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
            AuthorPostDto authorDto;

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44353/admin/api/authors/"+id);
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    authorDto = JsonConvert.DeserializeObject<AuthorPostDto>(responseStr);
                    return View(authorDto);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(AuthorPostDto authorDto)
        {
            using (HttpClient client = new HttpClient())
            {
                byte[] byteArr = null;

                if (authorDto.ImageFile.Length!=0)
                {
                    using (var mStream = new MemoryStream())
                    {
                        authorDto.ImageFile.CopyTo(mStream);
                        byteArr = mStream.ToArray();
                    }
                }
                else
                {
                    return BadRequest();
                }
                var byteArrContent = new ByteArrayContent(byteArr);
                byteArrContent.Headers.ContentType = MediaTypeHeaderValue.Parse(authorDto.ImageFile.ContentType);
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(authorDto.Name), Encoding.UTF8, "application/json"), "Name");
                multipartContent.Add(byteArrContent, "ImageFile", authorDto.ImageFile.FileName);

                string endpoint = "https://localhost:44353/admin/api/authors/"+ authorDto.Id;
                using (var Response = await client.PutAsync(endpoint, multipartContent))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("index", "author");
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
                var response = await client.DeleteAsync("https://localhost:44353/admin/api/authors/" + id);
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("index", "author");
                }
            }
            return RedirectToAction("index", "author");
        }

    }


}
