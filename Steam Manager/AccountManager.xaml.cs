using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SteamManager.Infrastructure;
using SteamAccount;

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
 
    public partial class AccountManager : Window
    {
        List<SteamAccountViewModel> steamAccounts = new List<SteamAccountViewModel>();
        public bool debug = false;
        public static string password;
        public static List<string> games = new List<string>();
        DriveInfo steamDrive = DriveInfo.GetDrives()[0];
        public IIOService _iOService { get; set; }
        public IStringEncryptionService _stringEncryptionService { get; set; }
        public AccountManager(string pass, IIOService iOService, IStringEncryptionService stringEncryptionService)
        {
            _iOService = iOService;
            _stringEncryptionService = stringEncryptionService;
            this.DataContext = steamAccounts;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            password = pass;
            string[] files = new string[0];
            System.IO.DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo d in drives)
            {
                if (Directory.Exists(d.Name + "Program Files (x86)\\Steam\\steamapps\\"))
                    steamDrive = d;
            }
            files = Directory.GetFiles(steamDrive.Name + "Program Files (x86)\\Steam\\steamapps\\");
            foreach (string f in files)
            {
                if (f.Contains(".acf"))
                {
                    char[] spl = { '"' };
                    string name1 = File.ReadAllText(f).Substring(File.ReadAllText(f).IndexOf("name") + 8).Split(spl)[0];
                    string appid = File.ReadAllText(f).Trim().Substring(File.ReadAllText(f).Trim().IndexOf("appid") + 9).Split(spl)[0];
                    int kl = Games_ComboBox.Items.Add(name1);
                    games.Insert(kl, appid);
                }
            }
            Run_Game.IsChecked = (GetSetting("startGame", "0") == "1" ? true : false);
            Run_Commandline.IsChecked = (GetSetting("launchParam", "0") == "1" ? true : false);
            Games_ComboBox.SelectedIndex = Games_ComboBox.Items.IndexOf(GetSetting("selectedGame", ""));
            Start_SteamLoad();
        }
        public void SetSetting(string index, string value)
        {
            index = index.Replace(":", "[/]");
            index = index.Replace(" ", "[%]");
            value = value.Replace(":", "[/]");
            value = value.Replace(" ", "[%]");
            if (debug) MessageBox.Show("Changing setting: " + index + " to: " + value);
            if (File.Exists("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\settings.conf"))
            {
                string sString = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\settings.conf");
                Dictionary<string, object> settings = JsonSerializer.Deserialize < Dictionary<string, object> > (sString);
                settings[index] = value;
                string writeJson = JsonSerializer.Serialize(settings);
                File.WriteAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\settings.conf", writeJson);
            }
            else
            {
                Dictionary<string, object> settings = new Dictionary<string, object>
                {
                    {index,value}
                };
                string writeJson = JsonSerializer.Serialize(settings);
                File.WriteAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\settings.conf", writeJson);
            }
        }
        public string GetSetting(string index, string Default)
        {
            index = index.Replace(":", "[/]");
            index = index.Replace(" ", "[%]");
            if (debug) MessageBox.Show("Getting setting: " + index);
            if (File.Exists("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\settings.conf"))
            {
                string sString = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\settings.conf");
                if (debug) MessageBox.Show("Setting JSON: " + sString);

                Dictionary<string, object> settings = JsonSerializer.Deserialize<Dictionary<string, object>>(sString);
                if (settings.ContainsKey(index))
                {
                    if (debug) MessageBox.Show("Setting: " + index + " Returned: " + (string)settings[index]);
                    if (debug) MessageBox.Show("Setting JSON: " + sString);

                    return ((string)settings[index]).Replace("[/]", ":").Replace("[%]", " ");
                }
                else
                {
                    if (debug) MessageBox.Show("Defaulted to: " + Default + " Type:2");

                    return Default;

                }
            }
            else
            {
                if (debug) MessageBox.Show("Defaulted to: " + Default + " Type:1");

                return Default;
            }
        }
        public void Start_SteamLoad()
        {

            if (Directory.Exists("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\Accounts"))
            {
                string[] files = Directory.GetFiles($"C:\\Users\\{Environment.UserName}\\Documents\\Steam Manager\\Accounts\\");
                char[] delimiters = { ';' };
                int count = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    string f = files[i];
                    string decryptor = _stringEncryptionService.DecryptString(password, File.ReadAllText(f));
                    string[] data = decryptor.Split(delimiters);
                    if (count == 1)
                    {
                        count = 0;
                        steamAccounts.Add(new SteamAccountViewModel { Name = data[0], UserName = data[1], Password = data[2], IsEveryOther = true, Index = i.ToString() });

                    }
                    else
                    {
                        count = 1;
                        steamAccounts.Add(new SteamAccountViewModel { Name = data[0], UserName = data[1], Password = data[2], IsEveryOther = false, Index = i.ToString() });

                    }

                    void btn3_Click(object sender2, RoutedEventArgs e2)
                    {
                        var Epass = new ExportPassword(null, null);
                        Epass.SetPass(password);
                        Epass.SetFile(f);
                        Epass.Owner = this;

                        Epass.Show();
                    }
                    void btn2_Click(object sender2, RoutedEventArgs e2)
                    {
                        // data[1] is the username
                        // data[2] is the password

                        foreach (var process in Process.GetProcessesByName("Steam"))
                        {
                            process.Kill();
                        }
                        Process.Start(steamDrive.Name + "Program Files (x86)\\Steam\\Steam.exe", $" -login {data[1]} {data[2]} {((Run_Game.IsChecked == true) ? ("-applaunch " + games[Games_ComboBox.SelectedIndex]) : "")} {((Run_Commandline.IsChecked == true) ? launchParams.Text : "") }");
                    }
                    void btn1_Click(object sender2, RoutedEventArgs e2)
                    {
                        var editwin = new EditWindow(data, password.Trim(), null, null);
                        editwin.Show();
                        editwin.Owner = this;
                    }

                }
            }
            else
            {
                Directory.CreateDirectory("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\Accounts\\");
            }
        }
        public void RefreshSteam()
        {
            Start_SteamLoad();
        }
        public void AddNewSteam(object sender, RoutedEventArgs e)
        {
            var win2 = new AddAccount(null, null);
            win2.SetPass(password);
            win2.Owner = this;
            win2.Show();
        }
        private void runGame_Checked(object sender, RoutedEventArgs e)
        {
            SetSetting("startGame", "1");
        }
        private void runGame_Unchecked(object sender, RoutedEventArgs e)
        {
            SetSetting("startGame", "0");
        }
        private void launchParams_Checked(object sender, RoutedEventArgs e)
        {
            SetSetting("launchParam", "1");
        }
        private void launchParams_Unchecked(object sender, RoutedEventArgs e)
        {
            SetSetting("launchParam", "0");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var IPass = new ImportPassword(null, null);
            IPass.SetPass(password);
            IPass.SetWindow(this);
            IPass.Show();
            IPass.Owner = this;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var Epass = new ExportPassword(null, null);
            Epass.SetPass(password);
            Epass.SetLibrary(true);
            Epass.Show();
            Epass.Owner = this;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetSetting("selectedGame", Games_ComboBox.SelectedItem.ToString());
            launchParams.Text = GetSetting("launchParam_" + Games_ComboBox.SelectedItem.ToString(), "");
        }
        private void launchParams_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Games_ComboBox.SelectedItem.ToString() != "") SetSetting("launchParam_" + Games_ComboBox.SelectedItem.ToString(), launchParams.Text);
        }

        private void Steam_StackPanel_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)e.OriginalSource).Content)
            {
                case ("Export"):
                    var Epass = new ExportPassword(null, null);
                    Epass.SetPass(password);
                    // Epass.SetFile(files[i]); // i is found in e.Tag and files is a local variable that needs its scope changed
                    Epass.Owner = this;
                    Epass.Show();
                    break;
                case ("Edit"):
                    break;
                case ("Login"):
                    break;
            }
            MessageBox.Show((string)((Button)e.OriginalSource).Tag);
        }
    }
}
