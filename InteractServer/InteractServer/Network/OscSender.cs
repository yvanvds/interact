using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Network
{
	public class OscSender
	{
		private Rug.Osc.OscSender sender = null;

		public OscSender(string address)
		{
			sender = new Rug.Osc.OscSender(System.Net.IPAddress.Parse(address), 11234);
			sender.Connect();
		}

		~OscSender()
		{
			if (sender != null)
			{
				sender.WaitForAllMessagesToComplete();
				sender.Close();
			}
		}

		public void Ping()
		{
			sender.Send(new Rug.Osc.OscMessage("/internal/ping"));
		}

		public void Connect()
		{
			sender.Send(new Rug.Osc.OscMessage("/internal/connect"));
		}

		public void Disconnect()
		{
			sender.Send(new Rug.Osc.OscMessage("/internal/disconnect"));
		}

		public void ProjectSet(string id, int version)
		{
			sender.Send(new Rug.Osc.OscMessage("/internal/project/set", id, version));
		}

		public void ProjectStart()
		{
			sender.Send(new Rug.Osc.OscMessage("/internal/project/start"));
		}

		public void ProjectStop()
		{
			sender.Send(new Rug.Osc.OscMessage("/internal/project/stop"));
		}

		public void ScreenStart(string screenID)
		{
			sender.Send(new Rug.Osc.OscMessage("/internal/screen/start", screenID));
			
		}
	}
}
