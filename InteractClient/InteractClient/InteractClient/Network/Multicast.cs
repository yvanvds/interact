﻿using Sockets.Plugin;
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
    UdpSocketMulticastClient receiver;
    bool joined = false;

    public static Multicast Get()
    {
      if (instance == null) instance = new Multicast();
      return instance;
    }

    public async void RequestServerList()
    {
      var msg = "INTERACT ID REQUEST";
      var msgBytes = Encoding.UTF8.GetBytes(msg);

      if (!joined)
      {
        await Join();
        joined = true;
      }
      await receiver.SendMulticastAsync(msgBytes);
    }

    public async void Disconnect()
    {
      if (joined)
      {
        await receiver.DisconnectAsync();
        joined = false;
      }
      receiver.Dispose();
      receiver = null;
    }

    private async Task Join()
    {
      if(receiver == null)
      {
        receiver = new UdpSocketMulticastClient();
        receiver.TTL = 5;
      }
      await receiver.JoinMulticastGroupAsync(Constants.MulticastAddress, Constants.MulticastPort);
    }

    
  }
}
