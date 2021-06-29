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
using EncryptStringSample;
using System.Diagnostics;
using System.Windows.Navigation;
using Microsoft;
using Microsoft.Win32;

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class ImportPassword : Window
    {

        public ImportPassword()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }


        string Password;
        public void SetPass(string p)
        {
            Password = p;


        }
        AccountManager wnd;
        public void SetWindow(AccountManager window)
        {
            wnd = window;
        }
        private void SetIPass(object sender, RoutedEventArgs e)
        {
            if (IPass.Password.Length < 8)
            {
                passError.Content = "Password must be atleast 8 characters";
            }
            else
            {
                var fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() == true)
                {
                    if (fileDialog.FileName.Contains(".enc"))
                    {
                        string content = File.ReadAllText(fileDialog.FileName);
                        char[] deli = { ';' };
                        string[] files = content.Split(deli);
                        for (int i = 0; i < files.Length; i++)
                        {

                            string decryptor = StringCipher.Decrypt(files[i], IPass.Password);
                            if (i == 0 && decryptor.Substring(0, 9) == "decrypted")
                            {
                                decryptor = decryptor.Substring(9);
                                string[] data = decryptor.Split(deli);
                                File.WriteAllText($"C:\\Users\\{Environment.UserName}\\Documents\\Steam Manager\\Accounts\\{data[0].Replace(".", "").Replace("/", "").Replace("\\", "")}.txt", StringCipher.Encrypt(decryptor, Password));
                            }
                            else if (i != 0)
                            {
                                string[] data = decryptor.Split(deli);
                                File.WriteAllText($"C:\\Users\\{Environment.UserName}\\Documents\\Steam Manager\\Accounts\\{data[0].Replace(".", "").Replace("/", "").Replace("\\", "")}.txt", StringCipher.Encrypt(decryptor, Password));
                            }
                            else
                            {
                                passError.Content = "Incorrect Password";
                                return;
                            }
                        }
                        wnd.Start_SteamLoad();
                        this.Close();
                    }
                    else if (System.IO.Path.GetFileName(fileDialog.FileName).Contains(".data"))
                    {
                        string contents = StringCipher.Decrypt(File.ReadAllText(fileDialog.FileName), IPass.Password);
                        if (contents.Substring(0, 9) == "decrypted")
                        {
                            File.WriteAllText($"C:\\Users\\{Environment.UserName}\\Documents\\Steam Manager\\Accounts\\{System.IO.Path.GetFileName(fileDialog.FileName).Replace(".data", ".txt").Replace("/", "").Replace("\\", "")}", StringCipher.Encrypt(contents.Substring(9), Password));
                            wnd.Start_SteamLoad();
                            this.Close();
                        }
                        else
                        {
                            passError.Content = "Incorrect Password";
                        }

                    }

                }
            }
        }
    }
}
