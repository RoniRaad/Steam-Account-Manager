﻿using SteamAccount;
using SteamManager.Application.Models;
using SteamManager.Infrastructure.Services;
using SteamManager.Infrastructure;
using SteamManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SteamManager.Application.ViewModels
{
    public class AccountManagerViewModel : IAccountManagerViewModel, INotifyPropertyChanged
    {
        private IIOService _iOService { get; set; }
        private IStringEncryptionService _stringEncryptionService;
        private ISteamService _steamService { get; set; }

        public AccountManagerViewModel(IIOService iOService, IStringEncryptionService stringEncryptionService, ISteamService steamService)
        {
            _stringEncryptionService = stringEncryptionService;
            _iOService = iOService;
            _steamService = steamService;
        }
        public List<string> Games { get; set; }
        ICollection<ISteamAccountViewModel> _steamAccountViewModels;

        public ICollection<ISteamAccountViewModel> SteamAccountViewModels
        {
            get { return _steamAccountViewModels; }
            set
            {
                if (_steamAccountViewModels == value)
                    return;
                _steamAccountViewModels = value;
                NotifyPropertyChanged(nameof(SteamAccountViewModels));
            }
        }

        public bool RunOnLogin { get; set; }
        public bool RunCommandLineArguments { get; set; }
        public string CommandLineArguments { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public ICollection<ISteamAccountModel> GetSteamAccountModels(string Password)
        {
            string decryptedData = _iOService.ReadData(Password);

            ICollection<ISteamAccountModel> steamAccountModels = JsonSerializer.Deserialize<ObservableCollection<SteamAccountModel>>(decryptedData).ToList<ISteamAccountModel>();

            return steamAccountModels;
        }
        public ICollection<ISteamAccountViewModel> GetSteamAccountViewModels(string Password)
        {
            ICollection<ISteamAccountViewModel> steamAccountViewModels = new ObservableCollection<ISteamAccountViewModel>();
            ICollection<ISteamAccountModel> steamAccountModels = GetSteamAccountModels(Password);

            int index = 0;
            foreach (ISteamAccountModel steamAccount in steamAccountModels)
            {
                ISteamAccountViewModel steamAccountViewModel = new SteamAccountViewModel();
                steamAccountViewModel.Index = index.ToString();
                steamAccountViewModel.Model = steamAccount;
                steamAccountViewModels.Add(steamAccountViewModel);
                index++;
            }

            return steamAccountViewModels;
        }

        public void AddSteamAccountModel(string password, ISteamAccountModel newAccount)
        {
            bool alreadyExists = false;
            ICollection<ISteamAccountModel> accounts = GetSteamAccountModels(password);

            foreach (ISteamAccountModel steamModel in accounts)
            {
                if (steamModel.UserName == newAccount.UserName)
                {
                    steamModel.UserName = newAccount.UserName;
                    steamModel.DisplayName = newAccount.DisplayName;
                    steamModel.Password = newAccount.Password;

                    alreadyExists = true;
                }
            }

            if (!alreadyExists)
            {
                accounts.Add(newAccount);
            }

            _iOService.UpdateData(JsonSerializer.Serialize(accounts), password);
        }

        public void LoginToSteamAccount(ISteamAccountViewModel steamAccount)
        {
            _steamService.LoginAsync(steamAccount.UserName, steamAccount.Password, "");
        }

        public void DeleteSteamAccount(string userName, string password)
        {
            ICollection<ISteamAccountModel> updatedModels = new ObservableCollection<ISteamAccountModel>();

            foreach (ISteamAccountModel steamAccount in GetSteamAccountModels(password))
                if (steamAccount.UserName != userName)
                    updatedModels.Add(steamAccount);

            _iOService.UpdateData(JsonSerializer.Serialize(updatedModels), password);
        }

        public void ExportSteamAccounts(string file, string[] userNames, string password, string exportPassword)
        {
            ICollection<ISteamAccountModel> exportAccounts = new ObservableCollection<ISteamAccountModel>();
            ICollection<ISteamAccountModel> accounts = GetSteamAccountModels(password);

            foreach (ISteamAccountModel steamModel in accounts)
                if (userNames.Contains(steamModel.UserName))
                    exportAccounts.Add(steamModel);


            string hashedExportPassword = _stringEncryptionService.Hash(exportPassword);
            string encryptedFileContents = _stringEncryptionService.EncryptString(hashedExportPassword, JsonSerializer.Serialize(exportAccounts));

            _iOService.WriteFile(file, encryptedFileContents);
        }

        public void ImportSteamAccounts(string file, string password, string importPassword)
        {
            string fileContents = _iOService.ReadFile(file);
            string decryptedContents = _stringEncryptionService.DecryptString(_stringEncryptionService.Hash(importPassword), fileContents);

            ICollection<ISteamAccountModel> importedAccounts = JsonSerializer.Deserialize<ObservableCollection<SteamAccountModel>>(decryptedContents).ToList<ISteamAccountModel>();

            foreach (ISteamAccountModel steamAccount in importedAccounts)
                AddSteamAccountModel(password, steamAccount);

        }
    }
}
