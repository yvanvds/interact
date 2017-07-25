using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.Implementation.Network
{
  // this class acts as a fudge for the actual osc implementation in every platform
  // osc is not done in xamarin.forms because it requires Windows.Net.IPAddress which
  // is not yet implemented.

  public class OscSender : Interact.Network.OscSender
  {
    Interface.IoscSender sender;

    public OscSender()
    {
      sender = DependencyService.Get<Interface.IoscSender>();
    }

    public override void Init(int port)
    {
      sender.Init(InteractClient.Network.Service.Get().ConnectedServer.Address, port);
    }

    public override void Init(string address, int port)
    {
      sender.Init(address, port);
    }

    public override void Send(string address, params object[] arguments)
    {
      sender.Send(address, arguments);
    }
  }
}
