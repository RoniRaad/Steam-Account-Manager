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
        private ILoginViewModel _loginViewModel { get; set; }
        private readonly AccountManager _accountManager;
        public MainWindow(ILoginViewModel loginViewModel, AccountManager accountManager)
        {
            _accountManager = accountManager;
            _loginViewModel = loginViewModel;
            DataContext = _loginViewModel;

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            _loginViewModel.Password = Password_Box.Password;

            string hashedPassword = _loginViewModel.HandleLogin();
            if (hashedPassword.Length != 0)
            {
                _accountManager.Password = hashedPassword;
                _accountManager.Show();
                Close();
            }

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!_accountManager.IsActive)
                _accountManager.Close();
        }
    }
}