using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Implementation
{
  class Values : Interact.Values
  {
    private Dictionary<string, object> storage = new Dictionary<string, object>();

    public override void Clear()
    {
      storage.Clear();
    }

    public override object Load(string key)
    {
      if (storage.ContainsKey(key))
      {
        return storage[key];
      }
      return null;
    }

    public override void Store(string key, object data)
    {
      if (storage.ContainsKey(key))
      {
        storage[key] = data;
      }
      else
      {
        storage.Add(key, data);
      }
    }
  }
}
