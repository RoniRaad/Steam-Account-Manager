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

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for AddAccount.xaml
    /// </summary>
    public partial class AddAccount : Window
    {

        public AddAccount()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }


        string Password;
        public void SetPass(string p)
        {
            Password = p;


        }
        private void NewAccount_Add(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\Accounts"))
            {
                Directory.CreateDirectory("C:\\Users\\" + Environment.UserName + "\\Documents\\Steam Manager\\Accounts\\");
            }
            string write = StringCipher.Encrypt(NewAccount_Name.Text + ";" + NewAccount_User.Text + ";" + NewAccount_Pass.Password, Password);
            File.WriteAllText($"C:\\Users\\{Environment.UserName}\\Documents\\Steam Manager\\Accounts\\{NewAccount_Name.Text}.txt", write);
            ((AccountManager)this.Owner).RefreshSteam();
            this.Close();
        }
    }
}