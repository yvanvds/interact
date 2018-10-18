using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Network
{
	public class Multicast
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

			try
			{
				if (!joined)
				{
					await Join();
					joined = true;
				}
				await receiver.SendMulticastAsync(msgBytes);
			} catch(Exception e)
			{
				Debug.WriteLine(e.Message);
			}
			
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
			if (receiver == null)
			{
				receiver = new UdpSocketMulticastClient();
				receiver.TTL = 5;
			}
			await receiver.JoinMulticastGroupAsync(Global.MulticastAddress, Global.MulticastPort);
		}
	}
}
