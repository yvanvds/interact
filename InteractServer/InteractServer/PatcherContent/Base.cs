using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatcherContent
{
  public class Base
  {
    public string Content;

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
