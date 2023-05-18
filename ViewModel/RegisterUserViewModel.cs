using System.ComponentModel.DataAnnotations;

namespace ContactWEB.ViewModels
{
    public class RegisterUserViewModel
    {
        // view validations
        [Required(ErrorMessage = "This field is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string LastName { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        public string Email { get; set; }
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{5,}$", ErrorMessage = "Password must have a minimum of five characters, at least one letter, one uppercase letter, one number and one special character")]
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{5,}$", ErrorMessage = "Password must have a minimum of five characters, at least one letter, one uppercase letter, one number and one special character")]
        [Required(ErrorMessage = "This field is required")]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [RegularExpression("(09)[0-9]{9}", ErrorMessage = "This is not a valid phone number")]
        [Required(ErrorMessage = "This field is required")]
        public string ContactNo { get; set; }
        public RegisterUserViewModel()
        {
            UserName = Email;
        }
    }
}