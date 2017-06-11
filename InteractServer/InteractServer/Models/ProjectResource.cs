using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
    

    public class ProjectResource
    {
        public string _name;
        public string Name { get; set; }

        [JsonIgnore][Ignore]
        public bool Tainted { get; set; }
    }
}
