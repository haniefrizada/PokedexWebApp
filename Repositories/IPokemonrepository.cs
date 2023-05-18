using PokedexWebApp.Models;

namespace PokedexWebApp.Repositories;

public interface IPokemonRepository
{
    Task<List<Pokemon>> GetAllPokemon(string token);
    Task<Pokemon> GetPokemonById(int id, string token);
    Task<Pokemon> AddPokemon(Pokemon newPokemon, string token);
    Task<Pokemon> UpdatePokemon(int pokemonId, Pokemon newPokemon, string token);
    Task DeletePokemon(int pokemonId, string token);
}