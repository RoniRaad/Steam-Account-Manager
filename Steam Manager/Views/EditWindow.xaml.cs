using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SteamAccount;
using SteamManager.Infrastructure;
using SteamManager.Models;
using SteamManager.Application.ViewModels;

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private string _password { get; set; }
        private string _currentUsername { get; set; }
        private IAccountManagerViewModel _accountManagerController { get; set; }
        private ISteamAccountModel _currentSteamAccount { get; set; }
        public EditWindow(IAccountManagerViewModel accountManagerController, ISteamAccountModel currentSteamAccount, string password)
        {
            _password = password;
            _accountManagerController = accountManagerController;
            _currentSteamAccount = currentSteamAccount;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.DataContext = _currentSteamAccount;
            InitializeComponent();
            
        }

        private void EditAccount_Add(object sender, RoutedEventArgs e)
        {
            _accountManagerController.AddSteamAccountModel(_password,_currentSteamAccount);
            this.Close();
        }

        private void EditAccount_Remove(object sender, RoutedEventArgs e)
        {
            _accountManagerController.DeleteSteamAccount(_currentSteamAccount.UserName, _password);
            this.Close();
        }
    }
}
