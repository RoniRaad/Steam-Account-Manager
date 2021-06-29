﻿using System;
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
    public partial class ExportPassword : Window
    {

        public ExportPassword()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }


        string Password;
        public void SetPass(string p)
        {
            Password = p;


        }
        string file;
        public void SetFile(string f)
        {
            file = f;


        }
        bool Library = false;
        public void SetLibrary(bool b)
        {
            Library = b;
        }
        private void SetEPass(object sender, RoutedEventArgs e)
        {
            if (EPass.Password.Length < 8)
            {
                passError.Content = "Password must be atleast 8 characters";
            }
            else
            {
                if (Library)
                {
                    string[] files = Directory.GetFiles($"C:\\Users\\{Environment.UserName}\\Documents\\Steam Manager\\Accounts\\");
                    char[] delimiters = { ';' };
                    string content = "";
                    int count = 0;
                    foreach (string f in files)
                    {
                        if (count == 0)
                        {
                            content = StringCipher.Encrypt("decrypted" + StringCipher.Decrypt(File.ReadAllText(f), Password), EPass.Password);
                            count = 1;
                        }
                        else
                        {
                            content = content + ";" + StringCipher.Encrypt(StringCipher.Decrypt(File.ReadAllText(f), Password), EPass.Password);
                        }

                    }
                    var fileDialog = new OpenFileDialog();
                    if (fileDialog.ShowDialog() == true)
                    {
                        
                        File.WriteAllText(fileDialog.FileName + "\\SteamManagerData.enc", content);
                        this.Close();

                    }
                }
                else
                {
                    var fileDialog = new OpenFileDialog();
                    if (fileDialog.ShowDialog() == true)
                    {

                        File.WriteAllText(fileDialog.FileName + "\\" + System.IO.Path.GetFileName(file).Replace(".txt", ".data").Replace("/", "").Replace("\\", ""), StringCipher.Encrypt("decrypted" + StringCipher.Decrypt(File.ReadAllText(file), Password), EPass.Password));
                        this.Close();

                    }
                }
            }
        }
    }
}