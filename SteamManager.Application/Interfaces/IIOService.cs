using SteamManager.Application.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SteamManager.Application
{
    public interface IIOService
    {
        void InitializeData(string password);
        string ReadData(string password);
        void UpdateData(string data, string password);
        bool ValidateData();
        string GetEncryptedUsername();
        public List<string[]> GetInstalledGamesManifest();
        public DriveInfo FindSteamDrive();
        void WriteFile(string file, string v);
        string ReadFile(string file);
        void SaveConfig(string config);
        string GetConfig();
    }
}