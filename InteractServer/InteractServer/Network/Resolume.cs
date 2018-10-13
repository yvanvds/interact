using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Network
{
	public class Resolume
	{
		public static Resolume Handle = null;

		private bool connected = false;
		public bool Connected => connected;

		private Osc.OscSender sender = null;

		public Resolume()
		{
			Handle = this;
		}

		public void Connect(string IP, int port)
		{
			if (Connected) DisConnect();
			/*if(IP.Equals("127.0.0.1"))
			{
				sender = new Rug.Osc.OscSender(System.Net.IPAddress.Loopback, 0, port);
			} else
			{
				sender = new Rug.Osc.OscSender(System.Net.IPAddress.Parse(IP), 0, port);
			}
			*/
			sender = new Osc.OscSender();

			try
			{
				sender.Init(IP, port);
			} catch(Exception e)
			{
				Log.Log.Handle.AddEntry("Resolume: " + e.Message);
			}
			
			connected = true;
		}

		public void DisConnect()
		{
			if(sender != null)
			{
				//sender.WaitForAllMessagesToComplete();
				//sender.Close();
				sender = null;
			}
			connected = false;
		}

		public void Send(string route, object[] arguments)
		{
			if (sender == null) return;
			
			sender.Send(new Osc.OscMessage(route, arguments));
#if DEBUG
			//Log.Log.Handle.AddEntry("Resolume: " + route);
#endif
		}

		~Resolume()
		{
			DisConnect();
		}
	}
}
