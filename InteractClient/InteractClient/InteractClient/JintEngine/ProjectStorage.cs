using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.JintEngine
{
    public class ProjectStorage
    {
        private Dictionary<string, object> storage = new Dictionary<string, object>();

        public void Clear()
        {
            storage.Clear();
        }

        public void Store(string key, object data)
        {
            if(storage.ContainsKey(key))
            {
                storage[key] = data;
            } else
            {
                storage.Add(key, data);
            }
        }

        public object Load(string key)
        {
            if(storage.ContainsKey(key))
            {
                return storage[key];
            }
            return null;
        }
    }
}
