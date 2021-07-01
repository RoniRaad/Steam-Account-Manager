namespace SteamManager.Models
{
    public interface ISteamAccountModel
    {
        string DisplayName { get; set; }
        string Password { get; set; }
        string UserName { get; set; }
    }
}