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
        public List<string> Games { get; set; }
        ICollection<ISteamAccountViewModel> _steamAccountViewModels;

        public ICollection<ISteamAccountViewModel> SteamAccountViewModels
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

        public bool RunOnLogin { get; set; }
        public bool RunCommandLineArguments { get; set; }
        public string CommandLineArguments { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
