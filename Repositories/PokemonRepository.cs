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
                var contact = JsonConvert.DeserializeObject<Pokemon>(content);
                return contact;
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
                throw new Exception("Failed to delete contact. Error: " + response.StatusCode);
            }
        }

        public async Task<List<Contact>> GetAllContacts(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await _httpClient.GetAsync("/Contacts");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var contacts = JsonConvert.DeserializeObject<List<Contact>>(content);
                return contacts ?? new();
            }

            return new();
        }

        public async Task<Contact> GetContactById(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var response = await _httpClient.GetAsync($"/GetContact/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<Contact>(content);
                return contact;
            }

            return null;
        }

        public async Task<Contact> UpdateContact(int id, Contact newContact, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var newContactAsString = JsonConvert.SerializeObject(newContact);
            var responseBody = new StringContent(newContactAsString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/UpdateContact/{id}", responseBody);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var contact = JsonConvert.DeserializeObject<Contact>(content);
                return contact;
            }
            else
            {
                throw new Exception("Failed to update contact. Error: " + response.StatusCode);
            }
        }

    }
}
