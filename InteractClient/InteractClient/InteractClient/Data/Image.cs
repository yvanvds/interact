using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.Data
{
    public class Image
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int Version { get; set; }
        
        public byte[] Content { get; set; }

        private ImageSource _ImageSource;
        [JsonIgnore]
        public ImageSource ImageSource
        {
            get
            {
                return _ImageSource;
            }
            set
            {
                _ImageSource = value;
            }
        }

        public void Deserialize(string data)
        {
            JsonConvert.PopulateObject(data, this);
            ImageSource = ImageSource.FromStream(() => new MemoryStream(Content));
        }

        
    }
}
