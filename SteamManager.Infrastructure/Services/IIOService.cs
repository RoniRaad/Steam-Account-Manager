namespace SteamManager.Infrastructure
{
    public interface IIOService
    {
        void InitializeData(string password);
        string ReadData(string password);
        void UpdateData(string data, string password);
        bool ValidateData();
        string GetEncryptedUsername();
    }
}