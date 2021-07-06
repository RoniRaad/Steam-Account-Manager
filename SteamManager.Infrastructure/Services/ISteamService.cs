namespace SteamManager.Infrastructure.Services
{
    public interface ISteamService
    {
        void Login(string userName, string password, string args);
        void StartSteam(string args);
        void StopSteam();
    }
}