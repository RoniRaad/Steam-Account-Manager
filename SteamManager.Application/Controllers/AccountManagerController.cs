using SteamAccount;
using SteamManager.Application.Models;
using SteamManager.Infrastructure;
using SteamManager.Infrastructure.Services;
using SteamManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SteamManager
{
    public class AccountManagerController : IAccountManagerController
    {
        private IIOService _iOService { get; set; }
        private IStringEncryptionService _stringEncryptionService;
        private ISteamService _steamService { get; set; }

        public AccountManagerController(IIOService iOService, IStringEncryptionService stringEncryptionService, ISteamService steamService)
        {
            _stringEncryptionService = stringEncryptionService;
            _iOService = iOService;
            _steamService = steamService;
        }
        public ObservableCollection<SteamAccountModel> GetSteamAccountModels(string Password)
        {
            string decryptedData = _iOService.ReadData(Password);

            ObservableCollection<SteamAccountModel> steamAccountModels = JsonSerializer.Deserialize<ObservableCollection<SteamAccountModel>>(decryptedData);

            return steamAccountModels;
        }
        public ObservableCollection<SteamAccountViewModel> GetSteamAccountViewModels(string Password)
        {
            ObservableCollection<SteamAccountViewModel> steamAccountViewModels = new ObservableCollection<SteamAccountViewModel>();
            ObservableCollection<SteamAccountModel> steamAccountModels = GetSteamAccountModels(Password);

            int index = 0;
            foreach (SteamAccountModel steamAccount in steamAccountModels)
            {
                SteamAccountViewModel steamAccountViewModel = new SteamAccountViewModel();
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
            ObservableCollection<SteamAccountModel> accounts = GetSteamAccountModels(password);

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

            if (!alreadyExists) {
                accounts.Add(newAccount);
            }

            _iOService.UpdateData(JsonSerializer.Serialize(accounts), password);
        }

        public void LoginToSteamAccount(SteamAccountViewModel steamAccount)
        {
            _steamService.Login(steamAccount.UserName, steamAccount.Password, "");
        }

        public void DeleteSteamAccount(string userName, string password)
        {
            ObservableCollection<SteamAccountModel> updatedModels = new ObservableCollection<SteamAccountModel>();

            foreach (SteamAccountModel steamAccount in GetSteamAccountModels(password))
                if (steamAccount.UserName != userName)
                    updatedModels.Add(steamAccount);

            _iOService.UpdateData(JsonSerializer.Serialize(updatedModels), password);
        }
        
        public void ExportSteamAccounts(string file, string[] userNames, string password, string exportPassword, bool replace)
        {
            ExportAccountModel exportAccountModel = new ExportAccountModel();
            ObservableCollection<SteamAccountModel> exportAccounts = new ObservableCollection<SteamAccountModel>();
            ObservableCollection<SteamAccountModel> accounts = GetSteamAccountModels(password);

            foreach (SteamAccountModel steamModel in accounts)
                if (userNames.Contains(steamModel.UserName))
                    exportAccounts.Add(steamModel);

            exportAccountModel.accountModels = exportAccounts;
            exportAccountModel.ReplaceCurrentModels = replace;

            string hashedExportPassword = _stringEncryptionService.Hash(exportPassword);
            string encryptedFileContents = _stringEncryptionService.EncryptString(hashedExportPassword, JsonSerializer.Serialize(exportAccountModel));

            _iOService.WriteFile(file, encryptedFileContents);
        }

        public void ImportSteamAccounts(string file, string password, string importPassword)
        {
            string fileContents = _iOService.ReadFile(file);
            string decryptedContents = _stringEncryptionService.DecryptString(_stringEncryptionService.Hash(importPassword), fileContents);

            ExportAccountModel importedAccounts = JsonSerializer.Deserialize<ExportAccountModel>(decryptedContents);

            foreach (SteamAccountModel steamAccount in importedAccounts.accountModels)
                AddSteamAccountModel(password, steamAccount);

        }
    }
}
