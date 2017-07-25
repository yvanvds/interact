using InteractClient.Interface;
using InteractClient.iOS.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(OscSenderImplementation))]
namespace InteractClient.iOS.Implementation
{

  public class OscSenderImplementation : IoscSender
  {
    private Rug.Osc.OscSender sender = null;

    public OscSenderImplementation() { }

    ~OscSenderImplementation()
    {
      if(sender != null) {
        sender.WaitForAllMessagesToComplete();
        sender.Close();
      }
    }

    public string BaseAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Init(string address, int port)
    {
      sender = new Rug.Osc.OscSender(System.Net.IPAddress.Parse(address), port);
      sender.Connect();
    }

    public void Send(string address, params object[] arguments)
    {
      object[] o = new object[arguments.Length];
      for (int i = 0; i < arguments.Length; i++)
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

      Rug.Osc.OscMessage message = new Rug.Osc.OscMessage(address, o);

      sender?.Send(message);
    }
  }
}
