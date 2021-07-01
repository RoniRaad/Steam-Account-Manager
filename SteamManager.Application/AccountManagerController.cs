using SteamManager.Infrastructure;
using SteamManager.Models;
using System;
using System.Collections.Generic;
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
        public List<SteamAccountViewModel> GetSteamAccountViewModels(string Password)
        {
            List<SteamAccountViewModel> steamAccountViewModels = new List<SteamAccountViewModel>();

            string decryptedData = _iOService.ReadData(Password);
            List<SteamAccountModel> steamAccountModels = JsonSerializer.Deserialize<List<SteamAccountModel>>(decryptedData);

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
    }
}
