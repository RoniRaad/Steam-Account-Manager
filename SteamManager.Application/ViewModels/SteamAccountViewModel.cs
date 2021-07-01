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

        private bool isEveryOther = false;

        public bool IsEveryOther
        {
            set { isEveryOther = value; }
        }

        public string Name
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
        private string index;
        public string Index
        {
            get { return index; }
            set { index = value; }
        }
    }
}
