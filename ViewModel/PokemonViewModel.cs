using System.ComponentModel.DataAnnotations;

namespace PokedexWebApp.ViewModel
{
    public class PokemonViewModel    {
        [StringLength(6, MinimumLength = 4)]
        public string PokemonNo { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
