using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Network
{
	public class Sender
	{
		private OscSender sender = null;

		public Sender(string address)
		{
			sender = new OscSender(System.Net.IPAddress.Parse(address), 11234);
			sender.Connect();
		}

		~Sender()
		{
			if(sender != null)
			{
				sender.WaitForAllMessagesToComplete();
				sender.Close();
			}
		}

		public void Ping()
		{
			sender.Send(new OscMessage("/action/ping"));
		}

		public void Disconnect()
		{
			sender.Send(new OscMessage("/action/disconnect"));
		}

		public void Invoke(string methodName, params object[] arguments)
		{
			sender.Send(new OscMessage("/action/invoke", methodName, arguments));
		}

		public void ClientAdd(string ipAddress, Guid key, string username)
		{
			sender.Send(new OscMessage("/client/add", ipAddress, key, username));
		}

		public void ProjectStop()
		{
			sender.Send(new OscMessage("/project/stop"));
		}

		public void ProjectSet(Guid id, int version)
		{
			sender.Send(new OscMessage("/project/set", id, version));
		}

		public void ProjectConfig(Guid id, Dictionary<string, string> values)
		{
			sender.Send(new OscMessage("/project/config", id, values));
		}

		public void ScreenStop()
		{
			sender.Send(new OscMessage("/screen/stop"));
		}

		public void ScreenVersion(Guid projectID, Guid screenID, int version)
		{
			sender.Send(new OscMessage("/screen/version", projectID, screenID, version));
		}

		public void ScreenSet(Guid projectID, Guid screenID, string screenData)
		{
			sender.Send(new OscMessage("/screen/set", projectID, screenID, screenData));
		}

		public void ScreenStart(Guid screenID)
		{
			sender.Send(new OscMessage("/screen/start", screenID));
		}

		public void PatcherVersion(Guid projectID, Guid patcherID, int version)
		{
			sender.Send(new OscMessage("/patcher/version", projectID, patcherID, version));
		}

		public void PatcherSet(Guid projectID, Guid patcherID, string patcherData)
		{
			sender.Send(new OscMessage("/patcher/set", projectID, patcherID, patcherData));
		}

		

		public void ImageVersion(Guid projectID, Guid imageID, int version)
		{
			sender.Send(new OscMessage("/image/version", projectID, imageID, version));
		}

		public void ImageSet(Guid projectID, Guid imageID, string imageData)
		{
			sender.Send(new OscMessage("/image/set", projectID, imageID, imageData));
		}

		public void SoundfileVersion(Guid projectID, Guid soundfileID, int version)
		{
			sender.Send(new OscMessage("/soundfile/version", projectID, soundfileID, version));
		}

		public void SoundfileSet(Guid projectID, Guid soundfileID, string soundfileData)
		{
			sender.Send(new OscMessage("/soundfile/set", projectID, soundfileID, soundfileData));
		}

		
	}
}
