using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Data
{
    public class SoundFile 
    {
        public enum Type
        {
            WAV,
            OGG,
            MP3,
            UNKNOWN,
        }

        public string Name { get; set; }
        public int ID { get; set; }
        public int Version { get; set; }
        public Type FileType { get; set; }

        public byte[] Content { get; set; }

        public void Deserialize(string data)
        {
            JsonConvert.PopulateObject(data, this);
        }
    }
}
