
using System.Collections.Generic;

namespace SteamManager
{
    public interface IAccountManagerController
    {
        List<SteamAccountViewModel> GetSteamAccountViewModels(string Password);
    }
}