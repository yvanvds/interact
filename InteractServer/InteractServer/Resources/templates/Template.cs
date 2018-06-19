using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace InteractServer.Resources.templates
{
  static public class Template
  {
    static public string Get(string template)
    {
      var assembly = Assembly.GetExecutingAssembly();
      var resourceName = "InteractServer.Resources.templates." + template;

      using (Stream stream = assembly.GetManifestResourceStream(resourceName)) 
        using (StreamReader reader = new StreamReader(stream))
      {
        return reader.ReadToEnd();
      }
    }
  }
}
