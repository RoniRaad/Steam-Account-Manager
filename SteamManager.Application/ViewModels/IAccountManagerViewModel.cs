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
        ObservableCollection<SteamAccountViewModel> SteamAccountViewModels { get; set; }
    }
}