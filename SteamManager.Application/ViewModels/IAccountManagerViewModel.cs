using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SteamManager.Application.ViewModels
{
    public interface IAccountManagerViewModel
    {
        string commandLineArguments { get; set; }
        List<string> games { get; set; }
        bool runCommandLineArguments { get; set; }
        bool runOnLogin { get; set; }
        ObservableCollection<SteamAccountViewModel> SteamAccountViewModels { get; set; }
    }
}