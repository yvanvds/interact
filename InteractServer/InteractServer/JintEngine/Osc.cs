using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.JintEngine
{
  public class Osc
  {
    public Osc(int port)
    {
      sender = new Rug.Osc.OscSender(System.Net.IPAddress.Parse("127.0.0.1"), port);
      sender.Connect();
    }

    public Osc(string address, int port) {
      sender = new Rug.Osc.OscSender(System.Net.IPAddress.Parse(address), port);
      sender.Connect();
    }

    public string BaseAddress { get; set; }

    public void Send(string address, params object[] arguments)
    {
      try
      {
        sender.Send(new Rug.Osc.OscMessage(BaseAddress + address, arguments));
      } catch(Exception e)
      {
        Global.Log.AddEntry("Osc object: " + e.ToString());
      }
    }

    private Rug.Osc.OscSender sender;
  }
}
