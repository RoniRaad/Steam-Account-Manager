using SteamAccount;
using SteamManager.Application.Models;
using SteamManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SteamAccount.Application;
using SteamManager.Application.Model;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

namespace SteamManager.Application.ViewModels
{
    public class AccountManagerViewModel : IAccountManagerViewModel, INotifyPropertyChanged
    {
        private readonly IIOService _iOService;
        private readonly IStringEncryptionService _stringEncryptionService;
        private readonly ISteamService _steamService;
        [JsonIgnore]
        public List<SteamGame> Games { get; set; }
        public SteamGame SelectedGame { get; set; }
        public bool RunOnLogin { get; set; }
        public bool RunCommandLineArguments { get; set; }
        public string CommandLineArguments { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        [JsonIgnore]
        private ICollection<ISteamAccountViewModel> _steamAccountViewModels;
        [JsonIgnore]
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
        public AccountManagerViewModel(IIOService iOService, IStringEncryptionService stringEncryptionService, ISteamService steamService)
        {
            _stringEncryptionService = stringEncryptionService;
            _iOService = iOService;
            _steamService = steamService;

            SetSteamGames(_iOService.GetInstalledGamesManifest());
            LoadSettings();

        }

        public void SaveSettings()
        {
            _iOService.SaveConfig(JsonSerializer.Serialize(this));
        }
        class AccountManagerSettings
        {
            public bool RunOnLogin { get; set; }
            public SteamGame SelectedGame { get; set; }
            public bool RunCommandLineArguments { get; set; }
            public string CommandLineArguments { get; set; }
        }
        public void LoadSettings()
        {
            AccountManagerSettings saveView = JsonSerializer.Deserialize<AccountManagerSettings>(_iOService.GetConfig());
            RunOnLogin = saveView.RunOnLogin;
            SelectedGame = Games.Where((steamGame) => { return steamGame.AppId == saveView.SelectedGame.AppId; }).FirstOrDefault();
            RunCommandLineArguments = saveView.RunCommandLineArguments;
            CommandLineArguments = saveView.CommandLineArguments;

            NotifyPropertyChanged(nameof(SelectedGame));
        }

        public void SetSteamGames(List<string[]> steamGamesManifestLines)
        {
            List<SteamGame> steamGames = new List<SteamGame>();

            foreach (string[] gameManifestLines in steamGamesManifestLines)
            {
                SteamGame steamGame = new SteamGame();

                string gameNameLine = gameManifestLines[5];
                string gameName = Regex.Matches(gameNameLine, @"(?<="")(.*?)(?="")")[2].Value;

                string appIdLine = gameManifestLines[2];
                string appId = Regex.Matches(appIdLine, @"(?<="")(.*?)(?="")")[2].Value;

                steamGame.Name = gameName;
                steamGame.AppId = appId;
                steamGames.Add(steamGame);
            }

            Games = steamGames;
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICollection<SteamAccountModel> GetSteamAccountModels(string Password)
        {
            string decryptedData = _iOService.ReadData(Password);

            ICollection<SteamAccountModel> steamAccountModels = JsonSerializer.Deserialize<ObservableCollection<SteamAccountModel>>(decryptedData);

            return steamAccountModels;
        }

        public ICollection<ISteamAccountViewModel> GetSteamAccountViewModels(string Password)
        {
            ICollection<ISteamAccountViewModel> steamAccountViewModels = new ObservableCollection<ISteamAccountViewModel>();
            ICollection<SteamAccountModel> steamAccountModels = GetSteamAccountModels(Password);

            int index = 0;
            foreach (SteamAccountModel steamAccount in steamAccountModels)
            {
                ISteamAccountViewModel steamAccountViewModel = new SteamAccountViewModel();
                steamAccountViewModel.Index = index.ToString();
                steamAccountViewModel.Model = steamAccount;
                steamAccountViewModels.Add(steamAccountViewModel);
                index++;
            }

            return steamAccountViewModels;
        }

        public void AddSteamAccountModel(string password, SteamAccountModel newAccount)
        {
            bool alreadyExists = false;
            ICollection<SteamAccountModel> accounts = GetSteamAccountModels(password);

            foreach (SteamAccountModel steamModel in accounts)
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
            string args = "";
            if (RunOnLogin)
                args += $"-applaunch {SelectedGame.AppId}";

            if (RunCommandLineArguments)
                args += $" {CommandLineArguments}";

            _steamService.LoginAsync(steamAccount.UserName, steamAccount.Password, args);
        }

        public void DeleteSteamAccount(string userName, string password)
        {
            ICollection<SteamAccountModel> updatedModels = new ObservableCollection<SteamAccountModel>();

            foreach (SteamAccountModel steamAccount in GetSteamAccountModels(password))
                if (steamAccount.UserName != userName)
                    updatedModels.Add(steamAccount);

            _iOService.UpdateData(JsonSerializer.Serialize(updatedModels), password);
        }

        public void ExportSteamAccounts(string file, string[] userNames, string password, string exportPassword)
        {
            ICollection<SteamAccountModel> exportAccounts = new ObservableCollection<SteamAccountModel>();
            ICollection<SteamAccountModel> accounts = GetSteamAccountModels(password);

            foreach (SteamAccountModel steamModel in accounts)
                if (userNames.Contains(steamModel.UserName))
                    exportAccounts.Add(steamModel);


            string hashedExportPassword = _stringEncryptionService.Hash(exportPassword);
            string json = JsonSerializer.Serialize(exportAccounts);
            string encryptedFileContents = _stringEncryptionService.EncryptString(hashedExportPassword, json);

            _iOService.WriteFile(file, encryptedFileContents);
        }

        public void ImportSteamAccounts(string file, string password, string importPassword)
        {
            string fileContents = _iOService.ReadFile(file);
            string decryptedContents = _stringEncryptionService.DecryptString(_stringEncryptionService.Hash(importPassword), fileContents);

            ICollection<SteamAccountModel> importedAccounts = JsonSerializer.Deserialize<ObservableCollection<SteamAccountModel>>(decryptedContents).ToList<SteamAccountModel>();

            foreach (SteamAccountModel steamAccount in importedAccounts)
                AddSteamAccountModel(password, steamAccount);

        }
    }
}
