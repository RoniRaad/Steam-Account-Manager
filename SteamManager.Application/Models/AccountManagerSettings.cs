using SteamManager.Application.Model;

namespace SteamManager.Application.ViewModels
{
    public partial class AccountManagerViewModel
    {
        public class AccountManagerSettings
        {
            public bool RunOnLogin { get; set; }
            public SteamGame SelectedGame { get; set; }
            public bool RunCommandLineArguments { get; set; }
            public string CommandLineArguments { get; set; }
        }
    }
}
