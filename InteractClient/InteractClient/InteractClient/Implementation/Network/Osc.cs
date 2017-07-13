using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Implementation.Network
{
  public class Osc : Interact.Network.Osc
  {
    public override void Init(int port)
    {
      //sender = new Rug.Osc.OscSender(new System.Net.IPAddress(InteractClient.Network.Service.Get().ConnectedServer.Address), port);
      sender.Connect();
    }

    public override void Init(string address, int port)
    {
      throw new NotImplementedException();
    }

    public override void Send(string address, params object[] arguments)
    {
      throw new NotImplementedException();
    }
  }
}
