using SteamManager.Infrastructure;

namespace SteamManager.Application.Controllers
{
    public interface ILoginController
    {
        IIOService _iOService { get; set; }
        bool _isRegistered { get; set; }

        public string HandleLogin(LoginViewModel loginViewModel);
        LoginViewModel GetViewModel();
    }
}