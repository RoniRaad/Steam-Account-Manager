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

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for AddAccount.xaml
    /// </summary>
    public partial class AddAccount : Window
    {
        public SteamAccountViewModel _newAccount { get; set; }
        public IAccountManagerController _accountManagerController { get; set; }
        public AddAccount(IAccountManagerController accountManagerController)
        {
            _newAccount = new SteamAccountViewModel();
            _accountManagerController = accountManagerController;

            this.DataContext = _newAccount;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }


        string Password;
        public void SetPass(string p)
        {
            Password = p;
        }
        public void NewAccount_Add(object sender, RoutedEventArgs e)
        {
            _newAccount.IsEveryOther = false;
            _newAccount.Password = NewAccount_Pass.Password;
            _accountManagerController.AddSteamAccountViewModel(Password, _newAccount);
            ((AccManager)this.Owner).RefreshSteam();
            this.Close();
        }
    }
}
