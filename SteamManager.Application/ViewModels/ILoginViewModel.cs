using System.ComponentModel;

namespace SteamManager
{
    public interface ILoginViewModel : INotifyPropertyChanged
    {
        string ErrorMessage { get; set; }
        string Password { get; set; }
        string Title { get; set; }
        string Username { get; set; }
        public string HandleLogin();
    }
}