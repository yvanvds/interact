using InteractClient.Interface;
using InteractClient.iOS.Implementation;
using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(OscReceiverImplementation))]
namespace InteractClient.iOS.Implementation
{
  public class OscReceiverImplementation : IoscReceiver
  {
    private OscReceiver receiver = null;

    ~OscReceiverImplementation()
    {
      Stop();
    }

    public OscSocketState State => receiver.State;

    public void Init(int port)
    {
      if (receiver == null) receiver = new OscReceiver(port);
      else Network.Service.Get().WriteLog("OscReceiver: Already Initialized.");
    }

    public bool Start()
    {
      if (receiver == null)
      {
        Network.Service.Get().WriteLog("OscReceiver: Init must be called before Start");
        return false;
      }
      else
      {
        receiver.Connect();
        return true;
      }
    }

    public void Stop()
    {
      receiver?.Close();
    }

    public bool TryReceive(out OscMessage message)
    {
      OscPacket packet;
      if (receiver.TryReceive(out packet))
      {
        message = OscMessage.Parse(packet.ToString());
        return true;
      }
      message = OscMessage.Parse("");
      return false;
    }
  }
}
