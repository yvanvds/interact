using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Implementation.Network
{
  public class Server : Interact.Network.Server
  {
    public override string IpAddress => InteractClient.Network.Sender.Get().ServerAddress;
    public override string Name => InteractClient.Network.Sender.Get().ServerName;

    public override void Invoke(string MethodName, params object[] arguments)
    {
      InteractClient.Network.Sender.Get().InvokeMethod(MethodName, arguments);
    }

    public override void Log(string Message)
    {
      InteractClient.Network.Sender.Get().WriteLog(Message);
    }
  }
}
