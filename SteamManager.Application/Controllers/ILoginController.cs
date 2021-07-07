using SteamManager.Infrastructure;

namespace SteamManager.Application.Controllers
{
    public interface ILoginController
    {
        public string HandleLogin(ILoginViewModel loginViewModel);
        ILoginViewModel GetViewModel();
    }
}