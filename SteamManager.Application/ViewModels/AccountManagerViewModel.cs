using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamManager.Application.ViewModels
{
    public class AccountManagerViewModel : IAccountManagerViewModel, INotifyPropertyChanged
    {
        public List<string> games { get; set; }
        ObservableCollection<SteamAccountViewModel> _steamAccountViewModels;

        public ObservableCollection<SteamAccountViewModel> SteamAccountViewModels
        {
            get { return _steamAccountViewModels; }
            set
            {
                if (_steamAccountViewModels == value)
                    return;
                _steamAccountViewModels = value;
                NotifyPropertyChanged("SteamAccountViewModels");
            }
        }

        public bool runOnLogin { get; set; }
        public bool runCommandLineArguments { get; set; }
        public string commandLineArguments { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
