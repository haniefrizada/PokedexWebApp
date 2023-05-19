using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PokedexWebApp.Models
{
    public class Pokemon
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(6, MinimumLength = 4)]
        [JsonProperty("PokemonNo")]
        public string PokemonNo { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Pokemon Name")]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [JsonProperty("Type")]
        public string Type { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [JsonProperty("Description")]
        public string Description { get; set; }
    }
}
