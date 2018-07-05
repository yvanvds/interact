using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Project
{
    public class ConfigValues : JsonObject 
    {
        public string Name { get; set; }
        public Guid ID { get; set; }
        public string StartupScreen { get; set; }
    }
}
