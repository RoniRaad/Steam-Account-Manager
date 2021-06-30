using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using SteamManager;
using SteamAccount;
using SteamManager.Infrastructure;
using System.Text.Json;

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginViewModel _loginViewModel { get; set; }
        private IIOService _iOService { get; set; }
        private IStringEncryptionService _stringEncryptionService { get; set; }
        private AccountManager _accountManager { get; set; }
        public MainWindow(IIOService iOService, IStringEncryptionService stringEncryptionService, AccountManager accountManager)
        {
            _accountManager = accountManager;
            _iOService = iOService;
            _stringEncryptionService = stringEncryptionService;
            _loginViewModel = new LoginViewModel();
            this.DataContext = _loginViewModel;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            if (_iOService.ValidateData())
            {
                _loginViewModel.Title = "Login";
            }
            else
            {
                _loginViewModel.Title = "Register";
            }


        }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(null, null);
            }

        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {

 
            string Username = _loginViewModel.Username;
            string Password;

            Password = _stringEncryptionService.Hash((string)Password_Box.Password);

            string decryptData = _iOService.ReadData(Password);

            if (decryptData.Length == 0)
            {
                _loginViewModel.ErrorMessage = "Username or Password Incorrect!";
            }
            else
            {
                _accountManager.Password = Password;
                _accountManager.RefreshSteam();
                _accountManager.Show();
                this.Close();
            }

        }
    }
}