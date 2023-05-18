
using PokedexWebApp.ViewModels;

namespace PokedexWebApp.Repositories
{
    public interface IAccountRepository
    {
        Task<bool> SignUpUserAsync(RegisterUserViewModel user);
        Task<string> SignInUserAsync(LoginUserViewModel loginUserViewModel);
    }
}
