using SteamManager.Infrastructure;

namespace SteamManager.Application.Controllers
{
    public interface ILoginController
    {
        public string HandleLogin(LoginViewModel loginViewModel);
        LoginViewModel GetViewModel();
    }
}