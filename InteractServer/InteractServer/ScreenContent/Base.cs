using InteractServer;
using InteractServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenContent
{

    public class Base
    {
        [JsonIgnore]
        public bool Tainted { get; set; }

        public Base(string content)
        {
            DeSerialize(content);
            Tainted = false;
        }

        public String Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void DeSerialize(String data)
        {
            try
            {
                JsonConvert.PopulateObject(data, this);
            }
            catch(Exception)
            {

            }
            
        }
    }
}
