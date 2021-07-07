using SteamManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SteamManager.Application.ViewModels
{
    public interface IAccountManagerViewModel
    {
        string CommandLineArguments { get; set; }
        List<string> Games { get; set; }
        bool RunCommandLineArguments { get; set; }
        bool RunOnLogin { get; set; }
        ICollection<ISteamAccountViewModel> SteamAccountViewModels { get; set; }
        ICollection<ISteamAccountViewModel> GetSteamAccountViewModels(string Password);
        public void AddSteamAccountModel(string password, ISteamAccountModel newAccount);
        public void LoginToSteamAccount(ISteamAccountViewModel steamAccount);
        void DeleteSteamAccount(string userName, string password);
        public void ExportSteamAccounts(string file, string[] userNames, string password, string exportPassword);
        void ImportSteamAccounts(string fileName, string password1, string password2);
    }
}