using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Osc
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
          OnBundleReceived(bundle);
        }
        else
        {
          OscMessage message = packet as OscMessage;
          OnMessageReceived(message);
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

    private void OnBundleReceived(OscBundle bundle)
    {
      foreach (OscPacket packet in bundle.Contents)
      {
        if (packet is OscBundle)
        {
          OscBundle subBundle = packet as OscBundle;
          OnBundleReceived(subBundle);
        }
        else
        {
          OscMessage message = packet as OscMessage;
          OnMessageReceived(message);
        }
      }
    }

    private void OnMessageReceived(OscMessage message)
    {
      if (AddressOnMessageReceived.ContainsKey(message.Address.Contents))
      {
        AddressOnMessageReceived[message.Address.Contents].Invoke(this, new OSCMessageReceivedArgs(message));
      }
      else
      {
        DefaultOnMessageReceived?.Invoke(this, new OSCMessageReceivedArgs(message));
      }
    }
  }

  public class OSCMessageReceivedArgs
  {
    public OscMessage Message { get; }
    public OSCMessageReceivedArgs(OscMessage message)
    {
      Message = message;
    }
  }

}
