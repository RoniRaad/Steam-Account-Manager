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
    public partial class ExportPassword : Window
    {
        private string[] _selectedUserNames { get; set; }
        private IAccountManagerViewModel _accountManagerController { get; set; }
        private string _password { get; set; }
        public ExportPassword(IAccountManagerViewModel accountManagerController, string[] SelectedUserNames, string password)
        {
            _password = password;
            _accountManagerController = accountManagerController;
            _selectedUserNames = SelectedUserNames;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void SetEPass(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Dat files (*.dat)|*.dat|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
                _accountManagerController.ExportSteamAccounts(saveFileDialog.FileName, _selectedUserNames, _password, EPass.Password);
            this.Close();
        }
    }
}
