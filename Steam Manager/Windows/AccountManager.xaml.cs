using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SteamManager.Infrastructure;
using SteamAccount;

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
 
    public partial class AccountManager : Window
    {
        public string SerializedData { get; set; }
        private List<SteamAccountViewModel> _steamAccounts = new List<SteamAccountViewModel>();
        public bool debug = false;
        public string Password { private get; set; }
        public static List<string> games = new List<string>();
        DriveInfo steamDrive = DriveInfo.GetDrives()[0];
        public IIOService _iOService { get; set; }
        public IStringEncryptionService _stringEncryptionService { get; set; }
        public AccountManager(IIOService iOService, IStringEncryptionService stringEncryptionService)
        {
            _iOService = iOService;
            _stringEncryptionService = stringEncryptionService;
            this.DataContext = _steamAccounts;
           // _steamAccounts = JsonSerializer.Deserialize<List<SteamAccountViewModel>>(SerializedData);
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

           // StartSteamLoad();
        }
        
        private void StartSteamLoad()
        {
            _steamAccounts = JsonSerializer.Deserialize<List<SteamAccountViewModel>>(_iOService.ReadData(Password));
            this.DataContext = _steamAccounts;
        }
        public void RefreshSteam()
        {
            StartSteamLoad();
        }
        public void AddNewSteam(object sender, RoutedEventArgs e)
        {
            var win2 = new AddAccount(_iOService, _stringEncryptionService);
            win2.SetPass(Password);
            win2.Owner = this;
            win2.Show();
        }
        private void runGame_Checked(object sender, RoutedEventArgs e)
        {
            //SetSetting("startGame", "1");
        }
        private void runGame_Unchecked(object sender, RoutedEventArgs e)
        {
            //SetSetting("startGame", "0");
        }
        private void launchParams_Checked(object sender, RoutedEventArgs e)
        {
           // SetSetting("launchParam", "1");
        }
        private void launchParams_Unchecked(object sender, RoutedEventArgs e)
        {
           // SetSetting("launchParam", "0");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var IPass = new ImportPassword(_iOService, _stringEncryptionService);
            IPass.SetPass(Password);
            IPass.SetWindow(this);
            IPass.Show();
            IPass.Owner = this;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var Epass = new ExportPassword(_iOService, _stringEncryptionService);
            Epass.SetPass(Password);
            Epass.SetLibrary(true);
            Epass.Show();
            Epass.Owner = this;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // SetSetting("selectedGame", Games_ComboBox.SelectedItem.ToString());
            //launchParams.Text = GetSetting("launchParam_" + Games_ComboBox.SelectedItem.ToString(), "");
        }
        private void launchParams_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (Games_ComboBox.SelectedItem.ToString() != "") SetSetting("launchParam_" + Games_ComboBox.SelectedItem.ToString(), launchParams.Text);
        }

        private void Steam_StackPanel_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)e.OriginalSource).Content)
            {
                case ("Export"):
                    var Epass = new ExportPassword(_iOService, _stringEncryptionService);
                    Epass.SetPass(Password);
                    // Epass.SetFile(files[i]); // i is found in e.Tag and files is a local variable that needs its scope changed
                    Epass.Owner = this;
                    Epass.Show();
                    break;
                case ("Edit"):
                    break;
                case ("Login"):
                    break;
            }
            MessageBox.Show((string)((Button)e.OriginalSource).Tag);
        }
    }
}
