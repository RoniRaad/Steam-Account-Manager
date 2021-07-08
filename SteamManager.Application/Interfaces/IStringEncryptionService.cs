namespace SteamAccount.Application
{
    public interface IStringEncryptionService
    {
        (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
        string DecryptString(string key, string cipherText);
        string EncryptString(string key, string plainText);
        string Hash(string password);
    }
}