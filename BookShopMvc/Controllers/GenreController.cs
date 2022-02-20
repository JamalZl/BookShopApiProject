using BookShopMvc.DTOs.GenreDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookShopMvc.Controllers
{
    public class GenreController : Controller
    {
        public async Task<IActionResult> Index()
        {
            GenreListDto genreListDto;

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44353/admin/api/genres");

                var responseStr = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    genreListDto = JsonConvert.DeserializeObject<GenreListDto>(responseStr);
                    return View(genreListDto);
                }
            }
            return RedirectToAction("index", "home");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GenrePostDto genreDto)
        {
            if (!ModelState.IsValid) return View();
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(genreDto), Encoding.UTF8, "application/json");
                string endpoint = "https://localhost:44353/admin/api/genres";
                using (var Response = await client.PostAsync(endpoint, content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("index", "genre");
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
            GenrePostDto genreDto;

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44353/admin/api/genres/" + id);
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    genreDto = JsonConvert.DeserializeObject<GenrePostDto>(responseStr);
                    return View(genreDto);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(GenrePostDto genreDto)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(genreDto), Encoding.UTF8, "application/json");
                string endpoint = "https://localhost:44353/admin/api/genres/" + genreDto.Id;
                using (var Response = await client.PutAsync(endpoint, content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("index", "genre");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync("https://localhost:44353/admin/api/genres/" + id);
                var responseStr = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("index", "genre");
                }
            }
            return RedirectToAction("index", "genre");
        }
    }
}
