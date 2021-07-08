using System.Threading.Tasks;

namespace SteamManager.Application
{
    public interface ISteamService
    {
        Task LoginAsync(string userName, string password, string args);
        void StartSteam(string args);
        void StopSteam();
    }
}