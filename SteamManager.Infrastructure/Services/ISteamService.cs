using System.Threading.Tasks;

namespace SteamManager.Infrastructure.Services
{
    public interface ISteamService
    {
        Task LoginAsync(string userName, string password, string args);
        void StartSteam(string args);
        void StopSteam();
    }
}