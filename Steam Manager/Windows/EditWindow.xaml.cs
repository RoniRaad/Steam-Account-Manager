using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SteamAccount;
using SteamManager.Infrastructure;

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public static string[] data;
        string Password;
        public IOService _iOService { get; set; }
        public IStringEncryptionService _stringEncryptionService { get; set; }
        public EditWindow(string[] pass, string password, IOService iOService, IStringEncryptionService stringEncryptionService)
        {
            _iOService = iOService;
            _stringEncryptionService = stringEncryptionService;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            data = pass;
            EditAccount_Name.Text = data[0];
            EditAccount_User.Text = data[1];
            this.Password = password;
        }

        private void EditAccount_Add(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\Accounts"))
            {
                Directory.CreateDirectory("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\Accounts\\");
            }
            File.Delete($"C:\\Users\\{Environment.UserName}\\Documents\\Steam Manager\\Accounts\\{data[0]}.txt");
            string write = _stringEncryptionService.EncryptString(Password, EditAccount_Name.Text + ";" + EditAccount_User.Text + ";" + ((EditAccount_Pass.Password != "Password") ? EditAccount_Pass.Password : data[2]));
            File.WriteAllText($"C:\\Users\\{Environment.UserName}\\Documents\\Steam Manager\\Accounts\\{EditAccount_Name.Text}.txt", write);
            ((AccManager)this.Owner).RefreshSteam();
            this.Close();
        }

        private void EditAccount_Remove(object sender, RoutedEventArgs e)
        {
            File.Delete($"C:\\Users\\{Environment.UserName}\\Documents\\Steam Manager\\Accounts\\{data[0]}.txt");
            ((AccManager)this.Owner).RefreshSteam();
            this.Close();
        }
    }
}
