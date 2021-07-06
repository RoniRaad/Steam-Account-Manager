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
using SteamManager.Application.ViewModels;
using System.Collections.ObjectModel;

namespace SteamManager
{
    public partial class AccountManager : Window
    {
        private IAccountManagerController _acccountManagerController { get; set; }
        public string Password { get; set; }
        private IAccountManagerViewModel _accountManagerViewModel { get; set; }
        public AccountManager(IAccountManagerController acccountManagerController, IAccountManagerViewModel accountManagerViewModel)
        {
            _accountManagerViewModel = accountManagerViewModel;
            _accountManagerViewModel.SteamAccountViewModels = new ObservableCollection<SteamAccountViewModel>();
            _acccountManagerController = acccountManagerController;

            this.DataContext = _accountManagerViewModel;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            InitializeComponent();
        }

        private void StartSteamLoad()
        {
            _accountManagerViewModel.SteamAccountViewModels = _acccountManagerController.GetSteamAccountViewModels(Password);
        }
        public void RefreshSteam()
        {
            StartSteamLoad();
        }
        public void AddNewSteam(object sender, RoutedEventArgs e)
        {
            var addAccountPrompt = new AddAccount(_acccountManagerController, Password);
            addAccountPrompt.Owner = this;
            addAccountPrompt.Show();
        }
        private void Steam_StackPanel_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)e.OriginalSource;
            switch (clickedButton.Content)
            {
                case ("Export"):
                    break;
                case ("Edit"):
                    Window editWindow = (new EditWindow(_acccountManagerController, ((SteamAccountViewModel)clickedButton.DataContext).Model, Password));
                    editWindow.Owner = this;
                    editWindow.Show();
                    break;
                case ("Login"):
                    _acccountManagerController.LoginToSteamAccount((SteamAccountViewModel)clickedButton.DataContext);
                    break;
            }
        }
    }
}
