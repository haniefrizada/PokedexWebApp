using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PokedexWebApp.Models
{
    public class Pokemon
    {
        public int Id { get; set; }

        [StringLength(6, MinimumLength = 4)]
        [Required(ErrorMessage = "Pokemon number is required.")]
        [RegularExpression("^(#\\d+|\\d+)$", ErrorMessage = "Pokemon number should start with '#' symbol and contain only digits or should contain only digits.")]
        public string PokemonNo { get; set; }

        [DisplayName("Pokemon Name")]
        [Required(ErrorMessage = "Pokemon Name is required.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "The {0} field should only contain letters and spaces. It cannot contain numbers.")]
        public string Name { get; set; }

        [DisplayName("Pokemon Type")]
        [Required(ErrorMessage = "Pokemon Type is required.")]
        [RegularExpression("^[^0-9]+$", ErrorMessage = "Pokemon Type cannot contain numbers.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
    }
}
