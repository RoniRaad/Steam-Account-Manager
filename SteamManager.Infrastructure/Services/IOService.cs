using SteamAccount.Application;
using SteamManager.Application;
using SteamManager.Application.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SteamManager.Infrastructure
{
    public class IOService : IIOService
    {
        private string _dataPath { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Steam Manager";
        private IStringEncryptionService _stringEncryptionService { get; set; }
        public IOService(IStringEncryptionService stringEncryptionService)
        {
            _stringEncryptionService = stringEncryptionService;
        }

        public bool ValidateData()
        {

            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }
            if (!File.Exists($"{_dataPath}\\data.dat"))
                return false;
            else
                return true;

        }

        public void UpdateData(string data, string password)
        {
            File.WriteAllText($"{_dataPath}\\data.dat", _stringEncryptionService.EncryptString(password ,data));
        }

        public string ReadData(string password)
{
            if (!File.Exists($"{_dataPath}\\data.dat"))
            {
                    InitializeData(password);
                    return "[]";

            }

            string encryptedData = File.ReadAllText($"{_dataPath}\\data.dat");
            string decryptedData = _stringEncryptionService.DecryptString(password, encryptedData);
            return _stringEncryptionService.DecryptString(password, encryptedData);
        }
        public void InitializeData(string password)
        {
            File.WriteAllText($"{_dataPath}\\data.dat", _stringEncryptionService.EncryptString(password, "[]"));
        }

        public string GetEncryptedUsername()
        {
            try
            {
                return File.ReadAllText($"{_dataPath}\\username.txt");
            }
            catch
            {
                return "";
            }
        }

        public DriveInfo FindSteamDrive()
        {
            DriveInfo steamDrive = null;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (Directory.Exists($"{drive.RootDirectory}\\Program Files (x86)\\Steam"))
                {
                    steamDrive = drive;
                }
            }
            return steamDrive;
        }
        public void WriteFile(string filePath, string fileContents)
        {
            File.WriteAllText(filePath, fileContents);
        }

        public string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public void SaveConfig(string contents)
        {
            WriteFile($"{_dataPath}\\config.conf", contents);
        }

        public string GetConfig()
        {
            try { 
                return ReadFile($"{_dataPath}\\config.conf");
            }
            catch
            {
                return "[]";
            }
        }
        public List<string[]> GetInstalledGamesManifest()
        {
            string[] steamAppFiles = Directory.GetFiles($"{FindSteamDrive()}\\Program Files (x86)\\Steam\\steamapps");
            List<string[]> steamGames = new List<string[]>();

            foreach (string file in steamAppFiles)
                if (file.Contains("appmanifest"))
                {
                    string[] fileContents = File.ReadAllLines(file);
                    steamGames.Add(fileContents);
                }

            return steamGames;
        }
    }
}
