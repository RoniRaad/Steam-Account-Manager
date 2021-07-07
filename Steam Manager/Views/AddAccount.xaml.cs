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
using System.Text.Json;
using System.Collections.ObjectModel;
using SteamManager.Models;
using SteamManager.Application.ViewModels;

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for AddAccount.xaml
    /// </summary>
    public partial class AddAccount : Window
    {
        private ISteamAccountModel _newAccount { get; set; }
        private IAccountManagerViewModel _accountManagerController { get; set; }
        private string _password { get; set; }
        public AddAccount(IAccountManagerViewModel accountManagerController, string password)
        {
            _password = password;
            _newAccount = new SteamAccountModel();
            _accountManagerController = accountManagerController;

            this.DataContext = _newAccount;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }


        public void NewAccount_Add(object sender, RoutedEventArgs e)
        {
            _newAccount.Password = NewAccount_Pass.Password;
            _accountManagerController.AddSteamAccountModel(_password, _newAccount);
            this.Close();
        }
    }
}
