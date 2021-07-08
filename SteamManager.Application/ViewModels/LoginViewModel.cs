using SteamAccount;
using SteamAccount.Application;
using SteamManager.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamManager
{
    public class LoginViewModel : ILoginViewModel
    {
        private string _errorMessage = "";
        public string Title { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        private IIOService _iOService { get; set; }
        private IStringEncryptionService _stringEncryptionService { get; set; }
        private bool _isRegistered { get; set; }

        public LoginViewModel(IIOService iOService, IStringEncryptionService stringEncryptionService)
        {
            _stringEncryptionService = stringEncryptionService;
            _iOService = iOService;
            Title = _iOService.ValidateData() ? "Login" : "Register";
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage == value)
                    return;
                _errorMessage = value;
                NotifyPropertyChanged(nameof(ErrorMessage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public class LoginResults
        {
            public int Passwoerd { get; }
            public bool Failed { get; }
        }


        /**
         * Returns the hashed password
         */
        public string HandleLogin()
        {
            string Password = _stringEncryptionService.Hash(Username + this.Password);
            string decryptData;

            try
            {
                decryptData = _iOService.ReadData(Password);
            }
            catch
            {
                decryptData = ""; // If we are unable to decode the data file then we are being given the wrong username/password
            }

            if (decryptData.Length == 0)
            {
                ErrorMessage = "Username or Password Incorrect!";
                return "";
            }
            else
            {
                return Password;
            }
        }
    }
}
