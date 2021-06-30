﻿using System;
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

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for AddAccount.xaml
    /// </summary>
    public partial class AddAccount : Window
    {
        public SteamAccountViewModel _newAccount { get; set; }
        public IOService _iOService { get; set; }
        public IStringEncryptionService _stringEncryptionService { get; set; }
        public AddAccount(IOService iOService, IStringEncryptionService stringEncryptionService)
        {
            this.DataContext = _newAccount;
            _iOService = iOService;
            _stringEncryptionService = stringEncryptionService;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }


        string Password;
        public void SetPass(string p)
        {
            Password = p;
        }
        private void NewAccount_Add(object sender, RoutedEventArgs e)
        {
            List<SteamAccountViewModel> accounts = JsonSerializer.Deserialize<List<SteamAccountViewModel>>(_iOService.ReadData(Password));
            accounts.Add(_newAccount);
            _iOService.UpdateData(JsonSerializer.Serialize(accounts));
            ((AccountManager)this.Owner).RefreshSteam();
            this.Close();
        }
    }
}
