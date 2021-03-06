using SteamManager.Models;

namespace SteamManager
{
    public interface ISteamAccountViewModel
    {
        string DisplayName { get; set; }
        string Index { get; set; }
        SteamAccountModel Model { get; set; }
        string Password { get; set; }
        string UserName { get; set; }
    }
}