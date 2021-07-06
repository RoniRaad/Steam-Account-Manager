using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamManager.Infrastructure.Services
{
    public class SteamService : ISteamService
    {
        private IIOService _iOService { get; set; }
        public SteamService(IIOService iOService)
        {
            _iOService = iOService;
        }
        public void StopSteam()
        {
            foreach (Process steamProcess in Process.GetProcessesByName("Steam"))
            {
                steamProcess.Kill();
            }
        }

        public void StartSteam(string args)
        {
            StopSteam();
            Process.Start($"{_iOService.FindSteamDrive()}\\Program Files (x86)\\Steam\\steam.exe", args);
        }

        public void Login(string userName, string password, string args)
        {
            StopSteam();
            StartSteam($"{args} -login {userName} {password}");
        }
    }
}
