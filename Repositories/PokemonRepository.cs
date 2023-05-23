using Newtonsoft.Json;
using PokedexWebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PokedexWebApp.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configs;

        public PokemonRepository(IConfiguration configs)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(httpClientHandler);
            _configs = configs;
            _httpClient.BaseAddress = new Uri("http://localhost:5087/api/pokemon");
        }

        public async Task<Pokemon> AddPokemon(Pokemon newPokemon, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            // Validate the newPokemon object
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(newPokemon, new ValidationContext(newPokemon), validationResults);

            if (!isValid)
            {
                // If the newPokemon object is not valid, throw an exception with the validation errors
                var errorMessages = validationResults.Select(vr => vr.ErrorMessage);
                throw new Exception(string.Join(", ", errorMessages));
            }

            var newTodoAsString = JsonConvert.SerializeObject(newPokemon);
            var requestBody = new StringContent(newTodoAsString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("", requestBody);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<Pokemon>(content);
                return contact;
            }

            return null;
        }


        public async Task DeletePokemon(int pokemonId, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await _httpClient.DeleteAsync($"/api/pokemon/{pokemonId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to delete pokemon. Error: " + response.StatusCode);
            }
        }

        public async Task<List<Pokemon>> GetAllPokemon(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await _httpClient.GetAsync("");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pokemonList = JsonConvert.DeserializeObject<List<Pokemon>>(content);
                return pokemonList ?? new List<Pokemon>();
            }

            return new List<Pokemon>();
        }

        public async Task<Pokemon> GetPokemonById(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await _httpClient.GetAsync($"/api/pokemon/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pokemon = JsonConvert.DeserializeObject<Pokemon>(content);
                return pokemon;
            }

            return null;
        }

        public async Task<Pokemon> GetPokemonByPokemonNo(string pokemonNo, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await _httpClient.GetAsync($"/api/pokemon/pokemonNo/{pokemonNo}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pokemon = JsonConvert.DeserializeObject<Pokemon>(content);
                return pokemon;
            }

            return null;
        }

        public async Task<Pokemon> UpdatePokemon(int pokemonId, Pokemon updatedPokemon, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var pokemonJson = JsonConvert.SerializeObject(updatedPokemon);
            var pokemonContent = new StringContent(pokemonJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/pokemon/{pokemonId}", pokemonContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var modifiedPokemon = JsonConvert.DeserializeObject<Pokemon>(responseContent);
            return modifiedPokemon;
        }
    }
}
