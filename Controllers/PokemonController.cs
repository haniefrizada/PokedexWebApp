using Microsoft.AspNetCore.Mvc;
using PokedexWebApp.Models;
using PokedexWebApp.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokedexWebApp.Controllers
{
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;

        public PokemonController(IPokemonRepository pokemonRepository)
        {
            _pokemonRepository = pokemonRepository;
        }

        public async Task<IActionResult> GetAllPokemon(string searchString)
        {
            string token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var pokemonList = await _pokemonRepository.GetAllPokemon(token);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower(); // Convert the search string to lowercase

                pokemonList = pokemonList.Where(p =>
                    p.Name.ToLower().Contains(searchString) ||
                    p.Type.ToLower().Contains(searchString) ||
                    p.PokemonNo.ToString().Contains(searchString)
                ).ToList();
            }

            return View(pokemonList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }
                
            var pokemon = await _pokemonRepository.GetPokemonById(id, token);
            if (pokemon == null)
                return NotFound();

            return View(pokemon);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Pokemon newPokemon)
        {
            var token = HttpContext.Session.GetString("JWToken");

            try
            {
                await _pokemonRepository.AddPokemon(newPokemon, token);

                TempData["SuccessNotification"] = "New Pokemon added successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorNotification"] = "Failed to add new Pokemon. Error: " + ex.Message;
            }

            return RedirectToAction("GetAllPokemon");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var pokemon = await _pokemonRepository.GetPokemonById(id, token);
            if (pokemon == null)
                return NotFound();

            return View(pokemon);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Pokemon updatedPokemon)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (id != updatedPokemon.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var modifiedPokemon = await _pokemonRepository.UpdatePokemon(id, updatedPokemon, token);
                TempData["SuccessNotification"] = "Pokemon details updated successfully!";
                return RedirectToAction(nameof(GetAllPokemon), new { id = modifiedPokemon.Id, token });
            }

            return View(updatedPokemon);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            await _pokemonRepository.DeletePokemon(id, token);
            TempData["SuccessNotification"] = "Pokemon deleted successfully!";
            return RedirectToAction(nameof(GetAllPokemon));
        }



    }
}
