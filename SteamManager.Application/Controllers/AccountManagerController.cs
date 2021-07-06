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
        private ISteamService _steamService { get; set; }

        public AccountManagerController(IIOService iOService, ISteamService steamService)
        {
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

        public void DeleteSteamAccount(string username, string password)
        {

        }
        
        public void ExportSteamAccounts(string file, string[] userNames, string password, bool replace)
        {
            ExportAccountModel exportAccountModel = new ExportAccountModel();
            ObservableCollection<SteamAccountModel> exportAccounts = new ObservableCollection<SteamAccountModel>();
            ObservableCollection<SteamAccountModel> accounts = GetSteamAccountModels(password);
            foreach (SteamAccountModel steamModel in accounts)
            {
                if (userNames.Contains(steamModel.UserName))
                {
                    exportAccounts.Add(steamModel);
                }
            }
            exportAccountModel.accountModels = exportAccounts;
            exportAccountModel.ReplaceCurrentModels = replace;
            _iOService.WriteFile(file, JsonSerializer.Serialize(exportAccountModel));
        }
    }
}
