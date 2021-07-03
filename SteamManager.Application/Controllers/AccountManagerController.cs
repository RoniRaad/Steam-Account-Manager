using SteamManager.Infrastructure;
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

        public AccountManagerController(IIOService iOService)
        {
            _iOService = iOService;
        }
        public ObservableCollection<SteamAccountViewModel> GetSteamAccountViewModels(string Password)
        {
            ObservableCollection<SteamAccountViewModel> steamAccountViewModels = new ObservableCollection<SteamAccountViewModel>();

            string decryptedData = _iOService.ReadData(Password);
            ObservableCollection<SteamAccountModel> steamAccountModels = JsonSerializer.Deserialize<ObservableCollection<SteamAccountModel>>(decryptedData);

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

        public void AddSteamAccountViewModel(string password, SteamAccountViewModel newAccount)
        {
            ObservableCollection<SteamAccountViewModel> accounts = GetSteamAccountViewModels(password);
            newAccount.Index = accounts.Count().ToString();
            accounts.Add(newAccount);
            _iOService.UpdateData(JsonSerializer.Serialize(accounts), password);
        }
    }
}
