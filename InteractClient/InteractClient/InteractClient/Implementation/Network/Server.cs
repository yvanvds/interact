using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Implementation.Network
{
  public class Server : Interact.Network.Server
  {
    public override string IpAddress => InteractClient.Network.Signaler.Get().ConnectedServer.Address;
    public override string Name => InteractClient.Network.Signaler.Get().ConnectedServer.Name;

    public override void Invoke(string MethodName, params object[] arguments)
    {
      InteractClient.Network.Signaler.Get().InvokeMethod(MethodName, arguments);
    }

    public override void Log(string Message)
    {
      InteractClient.Network.Signaler.Get().WriteLog(Message);
    }
  }
}
