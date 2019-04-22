using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
					writer.Write((Byte)NetworkMessage.EndOfMessage);
					writer.Flush();

					await this.sender.SendToAsync(stream.GetBuffer(), args.RemoteAddress, Constants.UdpPort);
				}
			};

            try
            {
                await receiver.JoinMulticastGroupAsync(Constants.MulticastAddress, Constants.MulticastPort);
                Byte[] data = Encoding.ASCII.GetBytes("ANOUNCE SERVER");
                await receiver.SendMulticastAsync(data);
            } catch(Exception e)
            {
                Log.Log.Handle.AddEntry("Error: " + e.Message);
            }
			
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
