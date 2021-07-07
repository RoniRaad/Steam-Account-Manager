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
using System.Diagnostics;
using System.Windows.Navigation;
using Microsoft;
using Microsoft.Win32;
using SteamAccount;
using SteamManager.Infrastructure;
using SteamManager.Application.ViewModels;

namespace SteamManager
{
    public partial class ImportPassword : Window
    {
        private IAccountManagerViewModel _accountManagerController { get; set; }
        private string _password { get; set; }
        public ImportPassword(IAccountManagerViewModel accountManagerController, string password)
        {
            _password = password;
            _accountManagerController = accountManagerController;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void SetIPass(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Dat files (*.dat)|*.dat|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                _accountManagerController.ImportSteamAccounts(openFileDialog.FileName, _password, IPass.Password);
            this.Close();
        }
    }
}
