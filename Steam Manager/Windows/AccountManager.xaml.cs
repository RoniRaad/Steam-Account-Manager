﻿using System.Collections.Generic;
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
            _acccountManagerController = acccountManagerController;

            this.DataContext = _accountManagerViewModel;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            InitializeComponent();
        }

        private void StartSteamLoad()
        {
            _accountManagerViewModel.SteamAccountViewModels = _acccountManagerController.GetSteamAccountViewModels(Password);
        }
        public void RefreshSteam(object sender, EventArgs e)
        {
            this.Show();
            StartSteamLoad();
        }
        public void AddNewSteam(object sender, RoutedEventArgs e)
        {
            var addAccountPrompt = new AddAccount(_acccountManagerController, Password);
            addAccountPrompt.Owner = this;
            addAccountPrompt.Show();
            addAccountPrompt.Closed += RefreshSteam;
        }
        private void Steam_StackPanel_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)e.OriginalSource;
            switch (clickedButton.Content)
            {
                case ("Export"):
                    Window exportWindow = new ExportPassword(_acccountManagerController, new string[] { ((ISteamAccountViewModel)clickedButton.DataContext).UserName }, Password);
                    exportWindow.Owner = this;
                    exportWindow.Show();
                    break;
                case ("Edit"):
                    Window editWindow = (new EditWindow(_acccountManagerController, ((ISteamAccountViewModel)clickedButton.DataContext).Model, Password));
                    editWindow.Owner = this;
                    editWindow.Show();
                    editWindow.Closed += RefreshSteam;
                    break;
                case ("Login"):
                    _acccountManagerController.LoginToSteamAccount((ISteamAccountViewModel)clickedButton.DataContext);
                    break;
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Window importWindow = new ImportPassword(_acccountManagerController, Password);
            importWindow.Show();
            importWindow.Owner = this;
            importWindow.Closed += RefreshSteam;

        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            string[] steamUserNames = new string[_accountManagerViewModel.SteamAccountViewModels.Count];
            int count = 0;
            foreach (ISteamAccountViewModel steamAccountView in _accountManagerViewModel.SteamAccountViewModels)
            {
                steamUserNames[count] = steamAccountView.UserName;
                count++;
            }
            Window exportWindow = new ExportPassword(_acccountManagerController, steamUserNames, Password);
            exportWindow.Show();
            exportWindow.Owner = this;
            exportWindow.Closed += RefreshSteam;
        }
    }
}