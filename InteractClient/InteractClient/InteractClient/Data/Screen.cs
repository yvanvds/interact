using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Data
{
    public class Screencontent
    {
        public string Content { get; set; }

        public Screencontent(string data)
        {
            JsonConvert.PopulateObject(data, this);
        }
    }

    public class Screen
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int Version { get; set; }
        public string Type { get; set; }

        public byte[] Content { get; set; }

        [JsonIgnore]
        public Screencontent screenContent;

        public void Deserialize(string data)
        {
            JsonConvert.PopulateObject(data, this);
            string script = Encoding.UTF8.GetString(Content, 0, Content.Length);
            screenContent = new Screencontent(script);
        }
    }
}
