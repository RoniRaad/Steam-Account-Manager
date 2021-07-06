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

namespace SteamManager
{
    public partial class ExportPassword : Window
    {
        private string[] _selectedUserNames { get; set; }
        public IAccountManagerController _accountManagerController { get; set; }
        public string Password { get; set; }
        public ExportPassword(IAccountManagerController accountManagerController, string[] SelectedUserNames)
        {
            _accountManagerController = accountManagerController;
            _selectedUserNames = SelectedUserNames;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void SetEPass(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                _accountManagerController.ExportSteamAccounts(saveFileDialog.FileName, _selectedUserNames, Password, false);

        }
    }
}
