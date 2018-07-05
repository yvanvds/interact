using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Data
{


  public class Patcher
  {
    public string Name { get; set; }
    public Guid ID { get; set; }
    public int Version { get; set; }
    
    public string Content { get; set; }

    public void Deserialize(string data)
    {
      JsonConvert.PopulateObject(data, this);
    }
  }
}
