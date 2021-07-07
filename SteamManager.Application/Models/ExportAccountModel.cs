using SteamManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamManager.Application.Models
{
    class ExportAccountModel
    {
        public ICollection<ISteamAccountModel> accountModels { get; set; }
        public bool ReplaceCurrentModels { get; set; }
    }
}
