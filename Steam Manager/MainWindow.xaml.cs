using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EncryptStringSample;
using System.IO; 
using System;
using SteamManager;
using SteamAccount;

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool debug = true;
        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            if (!File.Exists("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\logindata.txt"))
            {
                Main_Label.Content = "Register";
                if (debug) MessageBox.Show("No LoginData Requesting Register");
            }
            if (File.Exists("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\logindata.txt"))
            {
                string[] oldData = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\logindata.txt").Split(new char[] { ';' });
                if (oldData.Length > 1)
                {
                    if (debug) MessageBox.Show("Old Login Data Detected, Converting...");
                    File.WriteAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\logindata.txt", StringCipher.Encrypt(StringCipher.Decrypt(oldData[0], "IgerribUBbgriuetib8453jJI"), StringCipher.Decrypt(oldData[1], "IOASBHD8923jnbifbsfFjkigwre")));
                }

            }
            if (File.Exists("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\username.txt"))
            {
                Username_Box.Text = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\username.txt");
                Password_Box.Focus();
            }
        }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(null, null);
            }

        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {

            if (!Directory.Exists("C:\\Program Files (x86)\\Steam Manager\\"))
            {
                Directory.CreateDirectory("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\");
            }
 
            string Username = Username_Box.Text;
            string Password = Password_Box.Password;
            //MessageBox.Show(StringEncryptionService.DecryptString(StringEncryptionService.Hash(Password), StringEncryptionService.EncryptString(StringEncryptionService.Hash(Password), "Test")));

            string cuser = StringCipher.Encrypt(Username, Password);

            //if (debug) MessageBox.Show($"Username entered: {Username}");
            //if (debug) MessageBox.Show($"Password entered: {Password}");
            if (!File.Exists("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\logindata.txt"))
            {
                if (Password.Length < 8)
                {
                    Error_Label.Content = "Password must be at least 8 characters";
                    return;
                }
                else
                {
                    File.WriteAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\logindata.txt", cuser);
                    if (debug) MessageBox.Show($"Wrote new username to logindata: {cuser}");
                }

            }
            string data = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\logindata.txt");
            char[] delimiter = { ';' };
            string[] split = data.Split(delimiter);
            try
            {
                if (Username == StringCipher.Decrypt(split[0], Password))
                {
                    File.WriteAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\username.txt", Username);
                    var win1 = new AccountManager(Password);
                    win1.Show();
                    this.Close();
                    if (debug) MessageBox.Show($"Login Successful");
                }
                else
                {
                    Error_Label.Content = "Incorrect Username or Password";
                    if (debug) MessageBox.Show($"Incorrect Username or Password");
                }
            }
            catch (Exception ex)
            {
                Error_Label.Content = "Incorrect Username or Password";
                if (debug) MessageBox.Show(ex.ToString());
            }
        }
    }
}