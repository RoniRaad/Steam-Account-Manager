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
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class ImportPassword : Window
    {
        public ImportPassword(IIOService iOService, IStringEncryptionService stringEncryptionService)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void SetIPass(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
