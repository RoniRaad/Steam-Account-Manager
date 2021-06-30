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

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            this.DataContext = _loginViewModel;

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
            string Password = _stringEncryptionService.Hash(_loginViewModel.Password);

            string decryptData = _iOService.ReadData(Password);

            if (decryptData.Length == 0)
            {
                _loginViewModel.ErrorMessage = "Username or Password Incorrect!";
            }

            _accountManager.SerializedData = decryptData;
            _accountManager.Show();
            this.Close();
            //string data = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\logindata.txt");
            //char[] delimiter = { ';' };
            //string[] split = data.Split(delimiter);
            /*try
            {
                if (Username == StringCipher.Decrypt(split[0], Password))
                {
                    File.WriteAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\username.txt", Username);
                    var win1 = new AccountManager(Password);
                    win1.Show();
                    this.Close();
                    if (debug) MessageBox.Show($"Login Successful");
                }
                else
                {
                    Error_Label.Content = "Incorrect Username or Password";
                    if (debug) MessageBox.Show($"Incorrect Username or Password");
                }
            }
            catch (Exception ex)
            {
                Error_Label.Content = "Incorrect Username or Password";
                if (debug) MessageBox.Show(ex.ToString());
            }*/
        }
    }
}