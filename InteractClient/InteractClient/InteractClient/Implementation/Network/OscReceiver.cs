using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.Implementation.Network
{
  public class OscReceiver : Interact.Network.OscReceiver
  {
    private Osc.OscReceiver receiver;
    private bool started = false;

    private Dictionary<string, string> Routes = new Dictionary<string, string>();

    public OscReceiver()
    {
      receiver = new Osc.OscReceiver();

      receiver.DefaultOnMessageReceived += (sender, args) =>
      {
        string address = args.Message.Address.ToString();
        foreach (var entry in Routes)
        {
          if (address.StartsWith(entry.Key, StringComparison.CurrentCultureIgnoreCase))
          {
            JintEngine.Engine.Instance.Invoke(entry.Value, args.Message.Arguments.ToArray());
          }
        }
      };
    }

    ~OscReceiver()
    {
      Stop();
    }

    public override void Register(string address, string callbackFunction)
    {
      Routes[address] = callbackFunction;
    }

    public override void Start(int port)
    {
      if (started) return;
      receiver.Start(port);
      started = true;
    }

    public override void Stop()
    {
      if (!started) return;
      receiver.Stop();
      started = false;
    }

    
  }
}
