using SteamAccount;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SteamManager.Infrastructure
{
    public class IOService : IIOService
    {
        private string _dataPath { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Steam Manager";
        public IStringEncryptionService _stringEncryptionService { get; set; }
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


    }
}
