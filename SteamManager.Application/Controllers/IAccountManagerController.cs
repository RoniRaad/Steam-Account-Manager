
using SteamManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SteamManager
{
    public interface IAccountManagerController
    {
        ICollection<ISteamAccountViewModel> GetSteamAccountViewModels(string Password);
        public void AddSteamAccountModel(string password, ISteamAccountModel newAccount);
        public void LoginToSteamAccount(ISteamAccountViewModel steamAccount);
        void DeleteSteamAccount(string userName, string password);
        public void ExportSteamAccounts(string file, string[] userNames, string password, string exportPassword, bool replace);
        void ImportSteamAccounts(string fileName, string password1, string password2);
    }
}