
using InteractServer.Models;
using InteractServer.Pages;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace InteractServer.Network
{
  public class Multicast
  {
    UdpSocketMulticastClient receiver;
    UdpSocketClient sender;

    private bool connected = false;
    public bool Connected { get; }

    public async void Start()
    {
      if (connected) return;
      connected = true;

      receiver = new UdpSocketMulticastClient();
      receiver.TTL = 5;

      sender = new UdpSocketClient();

      receiver.MessageReceived += async (sender, args) =>
      {
        var data = Encoding.UTF8.GetString(args.ByteData, 0, args.ByteData.Length);

        if (data.Equals("INTERACT ID REQUEST"))
        {
          var stream = new MemoryStream();
          var writer = new BinaryWriter(stream, Encoding.UTF8);
          writer.Write((Byte)NetworkMessage.Acknowledge);
          writer.Write(Properties.Settings.Default.ServerName);
          writer.Write(Properties.Settings.Default.NetworkToken);
					writer.Write(Guid.NewGuid().ToString()); // in case the client does not have an ID, it can use this one
          writer.Write((Byte)NetworkMessage.EndOfMessage);
          writer.Flush();

          await this.sender.SendToAsync(stream.GetBuffer(), args.RemoteAddress, Constants.UdpPort);
        }
      };

      await receiver.JoinMulticastGroupAsync(Constants.MulticastAddress, Constants.MulticastPort);

      var msg = "TEST";
      var msgBytes = Encoding.UTF8.GetBytes(msg);
      await receiver.SendMulticastAsync(msgBytes);
    }

    void Verify()
    {
      
    }

    public async void Stop()
    {
      if (!connected) return;

      await receiver.DisconnectAsync();
      receiver.Dispose();

      await sender.DisconnectAsync();
      sender.Dispose();

      connected = false;
    }
  }
}
