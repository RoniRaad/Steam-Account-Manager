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
using SteamManager.Application.Controllers;

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
        private AccManager _accountManager { get; set; }
        public ILoginController _loginController { get; set; }
        public MainWindow(IIOService iOService, IStringEncryptionService stringEncryptionService, ILoginController loginController, AccManager accountManager)
        {
            _accountManager = accountManager;
            _iOService = iOService;
            _stringEncryptionService = stringEncryptionService;
            _loginController = loginController;
            _loginViewModel = _loginController.GetViewModel();
            this.DataContext = _loginViewModel;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();


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
            _loginViewModel.Password = Password_Box.Password;

            string hashedPassword = _loginController.HandleLogin(_loginViewModel);

            if (hashedPassword.Length != 0)
            {
                _accountManager.Password = hashedPassword;
                _accountManager.RefreshSteam();
                _accountManager.Show();
                this.Close();
            }

        }
    }
}