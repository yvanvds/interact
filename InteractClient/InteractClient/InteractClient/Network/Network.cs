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

    public static Network Get()
    {
      if (instance == null)
      {
        instance = new Network();
        instance.Start();
      }
      return instance;
    }


    private UdpSocketReceiver udpReciever = new UdpSocketReceiver();
    private UdpSocketClient udpSender = new UdpSocketClient();

    Guid ID = Guid.Empty;
    Server ActiveServer;

    DateTime LastMessageTime = new DateTime();

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

    /*private async Task parseTcpMessage(ITcpSocketClient client)
    {
        var reader = new BinaryReader(client.GetStream(), Encoding.UTF8);

        // verify this message comes from the server we're connected to
        if(ActiveServer != null)
        {
            if (!client.RemoteAddress.Equals(ActiveServer.Address)) return;
        }

        // read data
        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            switch((NetworkMessage)reader.ReadByte())
            {

                case NetworkMessage.SetCurrentProject:
                    {
                        Guid projectID = new Guid(reader.ReadBytes(16));
                        Data.Project.SetCurrent(ID);
                        break;
                    }


                case NetworkMessage.StoreScreen:
                    {
                        int screenID = reader.ReadInt32();
                        string content = reader.ReadString();
                        Data.Project.UpdateModel(screenID, content);
                        break;
                    }

                case NetworkMessage.StartScreen:
                    {
                        int screenID = reader.ReadInt32();
                        Engine.Instance.StartModel(screenID);
                        break;
                    }

                case NetworkMessage.StopScreen:
                    {
                        Engine.Instance.StopModel();
                        break;
                    }


                case NetworkMessage.EndOfMessage:
                    {
                        LastMessageTime = DateTime.Now;
                        return;
                    }
                default:
                    {
                        Debug.WriteLine("unknown message recieved");
                        break;
                    }
            }
        }
    }*/

    private void parseUdpMessage(String IpAddress, byte[] data)
    {
      var reader = new BinaryReader(new MemoryStream(data), Encoding.UTF8);
      while (reader.BaseStream.Position < reader.BaseStream.Length)
      {
        switch ((NetworkMessage)reader.ReadByte())
        {
          case NetworkMessage.Acknowledge:
            {
              // the server lets us know he exists
              string Name = reader.ReadString();
              string NetworkToken = reader.ReadString();

              Server target = Server.AddOrUpdate(Name, IpAddress);

              if (Global.CurrentPage is MainPage)
              {
                MainPage page = Global.CurrentPage as MainPage;
                Device.BeginInvokeOnMainThread(() => page.UpdateServerList());
              }

              if(NetworkToken != "")
              {
                if (NetworkToken.Equals(Settings.Current.Get<string>("NetworkToken")))
                {
                  // instant connect if client and server have the same token
                  Service.Get().ConnectAsync(target);
                }
              }
              break;
            }


          case NetworkMessage.EndOfMessage:
            {
              LastMessageTime = DateTime.Now;
              return;
            }
          default:
            {
              Debug.WriteLine("unknown message recieved");
              //LogPage.Get().AddEntry("Invalid message received from " + IpAddress);
              break;
            }
        }
      }
    }

    public void ConfirmPresence()
    {
      if (ID == Guid.Empty) return;

      LastMessageTime = DateTime.Now; // update because is triggered from server

      var writer = GetWriter();
      writer.Write((Byte)NetworkMessage.IamOnline);
      writer.Write(ID.ToByteArray());

    }

    public void Connect(Server server)
    {
      ActiveServer = server;
      var writer = GetWriter();
      writer.Write((Byte)NetworkMessage.RequestConnection);

    }

    public void JavaException(string error)
    {
      if (ID == Guid.Empty) return;
      var writer = GetWriter();
      writer.Write((Byte)NetworkMessage.JavaException);
      writer.Write(ID.ToByteArray());
      writer.Write(error);

    }

    public async void SendUdp(BinaryWriter writer)
    {
      if (ActiveServer == null) return;

      // only send if connection is alive
      if (!IsAlive())
      {
        // go back to root page
        //updatePage(false);
        return;
      }

      writer.Write((Byte)NetworkMessage.EndOfMessage);
      writer.Flush();
      MemoryStream stream = writer.BaseStream as MemoryStream;
      await udpSender.SendToAsync(stream.ToArray(), ActiveServer.Address, Constants.UdpPort);
    }

    public BinaryWriter GetWriter()
    {
      var stream = new MemoryStream();
      return new BinaryWriter(stream, Encoding.UTF8);
    }


    public bool IsAlive()
    {
      // connection is considered lost after 30 seconds
      var seconds = (DateTime.Now - LastMessageTime).TotalSeconds;
      return seconds < 30;
    }

  }
}
