using PokedexWebApp.Models;
using Newtonsoft.Json;
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
            _httpClient.BaseAddress = new Uri("https://localhost:5087");
        }
        public async Task<Pokemon> AddPokemon(Pokemon newPokemon, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var newTodoAsString = JsonConvert.SerializeObject(newPokemon);
            var requestBody = new StringContent(newTodoAsString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/AddPokemon", requestBody);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pokemon = JsonConvert.DeserializeObject<Pokemon>(content);
                return pokemon;
            }

            return null;
        }

        public async Task DeletePokemon(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var response = await _httpClient.DeleteAsync($"/DeletePokemon/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to delete pokemon. Error: " + response.StatusCode);
            }
        }

        public async Task<List<Pokemon>> GetAllPokemon(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await _httpClient.GetAsync("/Pokemons");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var contacts = JsonConvert.DeserializeObject<List<Pokemon>>(content);
                return contacts ?? new();
            }

            return new();
        }

        public async Task<Pokemon> GetPokemonById(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var response = await _httpClient.GetAsync($"/GetPokemon/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<Pokemon>(content);
                return contact;
            }

            return null;
        }

        public async Task<Pokemon> UpdatePokemon(int id, Pokemon newPokemon, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var newContactAsString = JsonConvert.SerializeObject(newPokemon);
            var responseBody = new StringContent(newContactAsString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/UpdatePokemon/{id}", responseBody);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<Pokemon>(content);
                return contact;
            }
            else
            {
                throw new Exception("Failed to update Pokemon Details. Error: " + response.StatusCode);
            }
        }

    }
}
