using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Osc
{
  public class OscReceiver : IDisposable
  {
    UdpSocketReceiver receiver;
    public event EventHandler<OSCMessageReceivedArgs> DefaultOnMessageReceived;
    public Dictionary<string, EventHandler<OSCMessageReceivedArgs>> AddressOnMessageReceived { get; set; }

    public OscReceiver()
    {
      AddressOnMessageReceived = new Dictionary<string, EventHandler<OSCMessageReceivedArgs>>();
      receiver = new UdpSocketReceiver();

      receiver.MessageReceived += (sender, args) =>
      {
        OscPacket packet = OscPacket.Parse(args.ByteData);
        if (packet is OscBundle)
        {
          OscBundle bundle = packet as OscBundle;
          OnBundleReceived(bundle, args.RemoteAddress);
        }
        else
        {
          OscMessage message = packet as OscMessage;
          OnMessageReceived(message, args.RemoteAddress);
        }
      };
    }

    public void Start(int port)
    {
      receiver.StartListeningAsync(port);
    }

    public void Stop()
    {
      receiver.StopListeningAsync();
    }

    public void Dispose()
    {
      Stop();
    }

    private void OnBundleReceived(OscBundle bundle, string origin)
    {
      foreach (OscPacket packet in bundle.Contents)
      {
        if (packet is OscBundle)
        {
          OscBundle subBundle = packet as OscBundle;
          OnBundleReceived(subBundle, origin);
        }
        else
        {
          OscMessage message = packet as OscMessage;
          OnMessageReceived(message, origin);
        }
      }
    }

    private void OnMessageReceived(OscMessage message, string origin)
    {
      if (AddressOnMessageReceived.ContainsKey(message.Address.Contents))
      {
        AddressOnMessageReceived[message.Address.Contents].Invoke(this, new OSCMessageReceivedArgs(message, origin));
      }
      else
      {
        DefaultOnMessageReceived?.Invoke(this, new OSCMessageReceivedArgs(message, origin));
      }
    }
  }

  public class OSCMessageReceivedArgs
  {
    public OscMessage Message { get; }
		public string Origin { get; }
    public OSCMessageReceivedArgs(OscMessage message, string origin)
    {
      Message = message;
			Origin = origin;
    }
  }

}
