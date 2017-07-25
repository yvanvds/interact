using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Implementation.Network
{
  public class Server : Interact.Network.Server
  {
    public override string IpAddress => "127.0.0.1";
    public override string Name => "Default Server Name";

    public override void Invoke(string MethodName, params object[] arguments)
    {
      JintEngine.Runner.Engine?.InvokeMethod(MethodName, arguments);
    }

    public override void Log(string Message)
    {
      Global.Log.AddEntry("Server Script: " + Message);
    }
  }
}
