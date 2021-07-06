
using SteamManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SteamManager
{
    public interface IAccountManagerController
    {
        ObservableCollection<SteamAccountViewModel> GetSteamAccountViewModels(string Password);
        public void AddSteamAccountModel(string password, SteamAccountModel newAccount);
        public void LoginToSteamAccount(SteamAccountViewModel steamAccount);
        void DeleteSteamAccount(string userName, string password);
        void ExportSteamAccounts(string fileName, string[] selectedUserNames, string password, bool v);
    }
}