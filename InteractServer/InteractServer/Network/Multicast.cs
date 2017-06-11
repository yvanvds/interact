
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

        public Multicast()
        {
            receiver = new UdpSocketMulticastClient();
            receiver.TTL = 5;

            receiver.MessageReceived += async (sender, args) =>
            {
                var data = Encoding.UTF8.GetString(args.ByteData, 0, args.ByteData.Length);

                if(data.Equals("INTERACT ID REQUEST"))
                {
                    var writer = Global.Network.GetWriter();
                    writer.Write((Byte)NetworkMessage.Acknowledge);
                    writer.Write("default server name");

                    await Global.Network.SendUdp(args.RemoteAddress, writer);
                }   
            };
        }

        public async void Disconnect()
        {
            await receiver.DisconnectAsync();
            receiver.Dispose();
        }

        public async void Join()
        {
            await receiver.JoinMulticastGroupAsync(Constants.MulticastAddress, Constants.MulticastPort);
        }
    }
}
