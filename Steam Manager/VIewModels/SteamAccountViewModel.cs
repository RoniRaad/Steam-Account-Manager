using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SteamManager
{
    public class SteamAccountViewModel
    {
        private bool isEveryOther = false;

        public bool IsEveryOther
        {
            set { isEveryOther = value; }
        }

        public SolidColorBrush BackgroundBrush
        {
            get { return (isEveryOther) ? Brushes.LightGray : Brushes.White; }
        }

        private string accountName;

        public string Name
        {
            get { return accountName; }
            set { accountName = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string index;

        public string Index
        {
            get { return index; }
            set { index = value; }
        }
    }
}
