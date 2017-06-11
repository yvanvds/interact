using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public abstract class JsonObject
    {

        public String Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void DeSerialize(String data)
        {
            try {
                JsonConvert.PopulateObject(data, this);
            } catch(JsonException e)
            {
                
            }
        }

        [JsonIgnore]
        public bool Tainted { get; set; } = false;
    }
}
