using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SteamManager.Models
{
    public class SteamAccountModel : ISteamAccountModel
    {
        [JsonConstructor]
        public SteamAccountModel(string userName, string displayName, string password)
        {
            UserName = userName;
            DisplayName = displayName;
            Password = password;

        }
        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }
    }
}
