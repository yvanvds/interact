using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.Implementation.Network
{
  public class OscSender : Interact.Network.OscSender
  {
    Osc.OscSender sender;

    public OscSender()
    {
      sender = new Osc.OscSender();
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
      object[] o = new object[arguments.Count()];
      for (int i = 0; i < arguments.Count(); i++)
      {
        if (arguments[i] is double)
        {
          double d = (double)arguments[i];
          o[i] = (float)d;
        }
        else
        {
          o[i] = arguments[i];
        }

      }

      sender.Send(new Osc.OscMessage(address, arguments));
    }
  }
}
