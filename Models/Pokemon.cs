using System.ComponentModel.DataAnnotations;

namespace PokedexWebApp.Models
{
    public class Pokemon
    {
        public int Id { get; set; }

        [StringLength(6, MinimumLength = 4)]
        public string PokemonNo { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public Pokemon()
        {
        }

        public Pokemon(int id, string pokemonNo, string name, string type, string description)
        {
            Id = id;
            PokemonNo = pokemonNo;
            Name = name;
            Type = type;
            Description = description;
        }
    }
}
