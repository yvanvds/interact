using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Implementation.Network
{
  public class OscSender : Interact.Network.OscSender
  {
    private Rug.Osc.OscSender sender = null;

    ~OscSender()
    {
      if(sender != null)
      {
        sender.WaitForAllMessagesToComplete();
        sender.Close();
      }
    }

    public override void Init(int port)
    {
      Init("127.0.0.1", port);
    }

    public override void Init(string address, int port)
    {
      sender = new Rug.Osc.OscSender(System.Net.IPAddress.Parse(address), port);
      sender.Connect();
    }

    public override void Send(string address, params object[] arguments)
    {
      Rug.Osc.OscMessage message;

      if (!AllowDouble)
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
        message = new Rug.Osc.OscMessage(address, o);
      } else
      {
        message = new Rug.Osc.OscMessage(address, arguments);
      }

      sender?.Send(message);
    }
  }
}
