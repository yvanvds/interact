using Acr.Settings;
using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Xamarin.Forms;
using InteractClient.JintEngine;
using Sockets.Plugin.Abstractions;

namespace InteractClient.Network
{
  public class Network
  {
    private static Network instance = null;

    private UdpSocketReceiver udpReciever = new UdpSocketReceiver();
    private UdpSocketClient udpSender = new UdpSocketClient();

    Guid ID = Guid.Empty;
    //ServerList ActiveServer;

    public static Network Get()
    {
      if (instance == null) {
        instance = new Network();
        instance.Start();
      }
      return instance;
    }

    public Network()
    {
      udpReciever.MessageReceived += (sender, args) =>
      {
        parseUdpMessage(args.RemoteAddress, args.ByteData);
      };

    }

    public async void Start()
    {
      await udpReciever.StartListeningAsync(Constants.UdpPort);
    }


    private async void parseUdpMessage(String IpAddress, byte[] data)
    {
      var reader = new BinaryReader(new MemoryStream(data), Encoding.UTF8);
      while (reader.BaseStream.Position < reader.BaseStream.Length)
      {
        switch ((NetworkMessage)reader.ReadByte())
        {
          case NetworkMessage.Acknowledge:
            {
              // the server acknowledges us
              string Name = reader.ReadString();
              string NetworkToken = reader.ReadString();

              ServerList target = ServerList.AddOrUpdate(Name, IpAddress);

              if (Global.CurrentPage is MainPage)
              {
                MainPage page = Global.CurrentPage as MainPage;
                Device.BeginInvokeOnMainThread(() => page.UpdateServerList());
              }

              // tokens can be used for auto connecting
              if (NetworkToken != "")
              {
                if (NetworkToken.Equals(CrossSettings.Current.Get<string>("NetworkToken")))
                {
                  // instant connect if client and server have the same token
                  Global.LookForServers = false;
                  await Signaler.Get().ConnectAsync(target);
                }
              }
              break;
            }
          case NetworkMessage.EndOfMessage:
            {
              return;
            }
          default:
            {
              Debug.WriteLine("unknown message recieved");
              break;
            }
        }
      }
    }
  }
}
