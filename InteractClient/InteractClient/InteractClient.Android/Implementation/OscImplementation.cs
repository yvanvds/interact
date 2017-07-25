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

using InteractClient.Interface;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using InteractClient.Droid.Implementation;

[assembly: Dependency(typeof(OscImplementation))]
namespace InteractClient.Droid.Implementation
{
  public class OscImplementation : IoscSender
  {
    private Rug.Osc.OscSender sender = null;

    public OscImplementation()
    {
    }

    ~OscImplementation()
    {
      if (sender != null)
      {
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
      object[] o = new object[arguments.Count()];
      for (int i = 0; i < arguments.Count(); i++)
      {
        if(arguments[i] is double)
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