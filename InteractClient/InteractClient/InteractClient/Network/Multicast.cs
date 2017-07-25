using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace InteractClient.Network
{
  class Multicast
  {
    private static Multicast instance = null;

    public static Multicast Get()
    {
      if (instance == null)
      {
        instance = new Multicast();
        //instance.Join();
      }

      return instance;
    }

    UdpSocketMulticastClient receiver;
    bool joined = false;

    public Multicast()
    {
      receiver = new UdpSocketMulticastClient();
      receiver.TTL = 5;

      receiver.MessageReceived += (sender, args) =>
      {
        var data = Encoding.UTF8.GetString(args.ByteData, 0, args.ByteData.Length);
        if (data.Equals("INTERACT REQUEST CONFIRMATION"))
        {
          Network.Get().ConfirmPresence();
        }
      };
    }

    public async void Disconnect()
    {
      if(joined)
      {
        await receiver.DisconnectAsync();
        joined = false;
      }
      
      receiver.Dispose();
    }

    public async Task Join()
    {
      await receiver.JoinMulticastGroupAsync(Constants.MulticastAddress, Constants.MulticastPort);
    }

    public async void RequestJoin()
    {
      var msg = "INTERACT JOIN REQUEST";
      var msgBytes = Encoding.UTF8.GetBytes(msg);

      await receiver.SendMulticastAsync(msgBytes);
    }

    public async void RequestServerList()
    {
      var msg = "INTERACT ID REQUEST";
      var msgBytes = Encoding.UTF8.GetBytes(msg);

      if(!joined)
      {
        await Join();
        joined = true;
      }
      await receiver.SendMulticastAsync(msgBytes);
    }
  }
}
