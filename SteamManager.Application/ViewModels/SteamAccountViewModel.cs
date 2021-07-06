using SteamManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SteamManager
{
    public class SteamAccountViewModel
    {
        public SteamAccountModel Model { get; set; } = new SteamAccountModel();

        public string DisplayName
        {
            get { return Model.DisplayName; }
            set { Model.DisplayName = value; }
        }

        public string UserName
        {
            get { return Model.UserName; }
            set { Model.UserName = value; }
        }

        public string Password
        {
            get { return Model.Password; }
            set { Model.Password = value; }
        }
        private string _index;
        public string Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
}
