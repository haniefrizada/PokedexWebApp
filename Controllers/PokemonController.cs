using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokedexWebApp.Models;

namespace PokedexWebApp.Controllers
{
    public class PokemonController : Controller
    {
        private readonly string apiBaseUrl = "http://localhost:5087/api/pokemon";

        // GET: Pokemon
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var pokemonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pokemon>>(data);
                    return View(pokemonList);
                }
                else
                {
                    // Handle error
                    return View("Error");
                }
            }
        }

        // GET: Pokemon/Details/5
        public async Task<ActionResult> Details(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var pokemon = Newtonsoft.Json.JsonConvert.DeserializeObject<Pokemon>(data);
                    return View(pokemon);
                }
                else
                {
                    // Handle error
                    return View("Error");
                }
            }
        }

        // GET: Pokemon/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pokemon/Create
        [HttpPost]
        public async Task<ActionResult> Create(Pokemon pokemon)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync("", pokemon);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Handle error
                        return View("Error");
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Pokemon/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var pokemon = Newtonsoft.Json.JsonConvert.DeserializeObject<Pokemon>(data);

                    if (pokemon != null)
                    {
                        return View(pokemon);
                    }
                    else
                    {
                        // Handle the case where the pokemon object is null
                        return View("Error");
                    }
                }
                else
                {
                    // Handle error
                    return View("Error");
                }
            }
        }


        // POST: Pokemon/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Pokemon pokemon)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PutAsJsonAsync($"{id}", pokemon);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Handle error
                        return View("Error");
                    }
                }
            }
            catch
            {
                return View();
            }
        }
        // GET: Pokemon/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var pokemon = Newtonsoft.Json.JsonConvert.DeserializeObject<Pokemon>(data);
                    return View(pokemon);
                }
                else
                {
                    // Handle error
                    return View("Error");
                }
            }
        }

        // POST: Pokemon/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.DeleteAsync($"{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Handle error
                        return View("Error");
                    }
                }
            }
            catch
            {
                return View();
            }
        }
    }
}


