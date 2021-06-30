namespace SteamManager.Infrastructure
{
    public interface IIOService
    {
        void InitializeData(string password);
        string ReadData(string password);
        void UpdateData(string data);
        bool ValidateData();
        string GetEncryptedUsername();
    }
}