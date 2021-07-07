using SteamAccount;
using SteamManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamManager.Application.Controllers
{
    public class LoginController : ILoginController
    {
        private IIOService _iOService { get; set; }
        private IStringEncryptionService _stringEncryptionService { get; set; }
        private bool _isRegistered { get; set; }

        public LoginController(IIOService iOService, IStringEncryptionService stringEncryptionService)
        {
            _stringEncryptionService = stringEncryptionService;
            _iOService = iOService;
            _isRegistered = _iOService.ValidateData();
        }

        public ILoginViewModel GetViewModel()
        {
            ILoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.Title = (_isRegistered) ? "Login" : "Register";
            return loginViewModel;
        }
        /**
         * Returns the hashed password
         */
        public string HandleLogin(ILoginViewModel loginViewModel)
        {
            string Password = _stringEncryptionService.Hash( loginViewModel.Username + loginViewModel.Password);
            string decryptData;
            try { 
                decryptData = _iOService.ReadData(Password);
            }
            catch
            {
                decryptData = ""; // If we are unable to decode the data file then we are being given the wrong username/password
            }

            if (decryptData.Length == 0)
            {
                loginViewModel.ErrorMessage = "Username or Password Incorrect!";
                return "";
            }
            else
            {
                return Password;
            }
        }
    }
}
