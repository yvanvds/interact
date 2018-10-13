using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Network
{
	public class OscSender
	{
		private Osc.OscSender sender = null;

		public OscSender(string address)
		{
			sender = new Osc.OscSender();
			sender.Init(address, 11234);
		}

		/*~OscSender()
		{
			if (sender != null)
			{
				try
				{
					sender.
					sender.WaitForAllMessagesToComplete();
					sender.Close();
				}
				catch (Exception e)
				{
					Log.Log.Handle.AddEntry(e.Message);
				}
			}
		}*/

		public void Ping()
		{
			try
			{
				sender.Send(new Osc.OscMessage("/internal/ping"));
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}

		}

		public void Connect()
		{
			try
			{
				sender.Send(new Osc.OscMessage("/internal/connect"));
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void Disconnect()
		{
			try
			{
				sender.Send(new Osc.OscMessage("/internal/disconnect"));
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void ProjectSet(string id, int version)
		{
			try
			{
				sender.Send(new Osc.OscMessage("/internal/project/set", id, version));
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void ProjectStart()
		{
			try
			{
				sender.Send(new Osc.OscMessage("/internal/project/start"));
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void ProjectStop()
		{
			try
			{
				sender.Send(new Osc.OscMessage("/internal/project/stop"));
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void ScreenStart(string screenID)
		{
			try
			{
				sender.Send(new Osc.OscMessage("/internal/screen/start", screenID));
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void GroupSet(string groupID, string startScreenID)
		{
			try
			{
				sender.Send(new Osc.OscMessage("/internal/group/set", groupID, startScreenID));
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}

		public void ToClient(string route, object[] arguments)
		{
			try
			{
				sender.Send(new Osc.OscMessage(route, arguments));
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
		}
	}
}
