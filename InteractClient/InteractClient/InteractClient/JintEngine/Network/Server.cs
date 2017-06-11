using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.JintEngine.Network
{
  public class Server
  {
    public void InvokeMethod(string name, params object[] arguments)
    {
      InteractClient.Network.Service.Get().InvokeMethod(name, arguments);
    }

  }
}
