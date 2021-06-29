namespace SteamManager.Infrastructure
{
    public interface IIOService
    {
        void InitializeData(string password);
        string ReadData(string password);
        void UpdateData();
        bool ValidateData();
        string GetEncryptedUsername();
    }
}