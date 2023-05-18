using PokedexWebApp.Models;
using PokedexWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;

namespace PokedexWebApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IPokemonRepository _repo;
        public ContactController(IPokemonRepository repo)
        {
            _repo = repo;
        }
        public async Task<IActionResult> GetAllPokemon()
        {
            string token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case when the token is not available
                return RedirectToAction("Login", "Account");
            }

            var pokemons = await _repo.GetAllPokemon(token);

            return View(pokemons);
        }

        public async Task<IActionResult> CreateAsync()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Pokemon newPokemon)
        {
            var token = HttpContext.Session.GetString("JWToken");

            await _repo.AddPokemon(newPokemon, token);
            return RedirectToAction("GetAllPokemon");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var pokemon = await _repo.GetPokemonById(id, token);

            if (pokemon is null)
                return NotFound();

            return View(pokemon);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Pokemon updatedPokemon)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");
                var pokemonId = updatedPokemon.Id;
                var updatedPokemonResult = await _repo.UpdatePokemon(pokemonId, updatedPokemon, token);

                if (updatedPokemonResult != null)
                {
                    // Handle successful update, if needed
                    return RedirectToAction("GetAllPokemon");
                }
                else
                {
                    // Handle failed update, if needed
                    ModelState.AddModelError(string.Empty, "Failed to update contact.");
                    return View(updatedPokemon);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception and return the View with the appropriate error message
                ModelState.AddModelError(string.Empty, "Failed to update Pokemon: " + ex.Message);
                return View(updatedPokemon);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            await _repo.DeletePokemon(id, token);
            return RedirectToAction("GetAllPokemon");
        }

        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var pokemon = await _repo.GetPokemonById(id, token);

            if (pokemon is null)
                return NotFound();

            return View(pokemon);
        }


    }
}
