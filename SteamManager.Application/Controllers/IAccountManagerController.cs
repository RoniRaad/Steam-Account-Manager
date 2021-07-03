
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SteamManager
{
    public interface IAccountManagerController
    {
        ObservableCollection<SteamAccountViewModel> GetSteamAccountViewModels(string Password);
        public void AddSteamAccountViewModel(string password, SteamAccountViewModel newAccount);
    }
}