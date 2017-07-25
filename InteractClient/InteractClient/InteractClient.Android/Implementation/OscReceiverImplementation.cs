using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using InteractClient.Droid.Implementation;
using InteractClient.Interface;
using Rug.Osc;

[assembly: Dependency(typeof(OscReceiverImplementation))]
namespace InteractClient.Droid.Implementation
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