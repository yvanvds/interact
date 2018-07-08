using Newtonsoft.Json;
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

		public void Connect()
		{
			sender.Send(new OscMessage("/action/connect"));
		}

		public void Disconnect()
		{
			sender.Send(new OscMessage("/action/disconnect"));
		}

		public void Invoke(string methodName, params object[] arguments)
		{
			sender.Send(new OscMessage("/action/invoke", methodName, arguments));
		}

		public void ClientAdd(Guid clientID, string ipAddress, string username)
		{
			try
			{
				sender.Send(new OscMessage("/client/add", clientID.ToString(), ipAddress, username));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) ClientAdd: " + e.Message);
			}
		}

		public void ClientRemove(Guid clientID)
		{
			try
			{
				sender.Send(new OscMessage("/client/remove", clientID.ToString()));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) ClientRemove: " + e.Message);
			}
		}

		public void ProjectStop()
		{
			sender.Send(new OscMessage("/project/stop"));
		}

		public void ProjectSet(Guid id, int version)
		{
			try
			{
				sender.Send(new OscMessage("/project/set", id.ToString(), version));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) ProjectSet: " + e.Message);
			}
		}

		/*public void ProjectConfig(Guid id, Dictionary<string, string> values)
		{
			try
			{
				sender.Send(new OscMessage("/project/config", id.ToString(), JsonConvert.SerializeObject(values)));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) ProjectConfig: " + e.Message);
			}
		}*/

		public void ScreenStop()
		{
			sender.Send(new OscMessage("/screen/stop"));
		}

		public void ScreenVersion(Guid projectID, Guid screenID, int version)
		{
			try
			{
				sender.Send(new OscMessage("/screen/version", projectID.ToString(), screenID.ToString(), version));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) ScreenVersion: " + e.Message);
			}
		}

		public void ScreenSet(Guid projectID, Guid screenID, string screenData)
		{
			try
			{
				sender.Send(new OscMessage("/screen/set", projectID.ToString(), ToBlob(screenData)));
			} catch (Exception e)
			{
				Global.Log.AddEntry("(Server) ScreenSet: " + e.Message);
			}
			
		}

		public void ScreenStart(Guid screenID)
		{
			try
			{
				sender.Send(new OscMessage("/screen/start", screenID.ToString()));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) ScreenStart: " + e.Message);
			}
		}

		public void PatcherVersion(Guid projectID, Guid patcherID, int version)
		{
			try
			{
				sender.Send(new OscMessage("/patcher/version", projectID.ToString(), patcherID.ToString(), version));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) PatcherVersion: " + e.Message);
			}
		}

		public void PatcherSet(Guid projectID, Guid patcherID, string patcherData)
		{
			try
			{
				sender.Send(new OscMessage("/patcher/set", projectID.ToString(), patcherID.ToString(), patcherData));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) PatcherSet: " + e.Message);
			}
		}

		

		public void ImageVersion(Guid projectID, Guid imageID, int version)
		{
			try
			{
				sender.Send(new OscMessage("/image/version", projectID.ToString(), imageID.ToString(), version));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) ImageVersion: " + e.Message);
			}
		}

		public void ImageSet(Guid projectID, Guid imageID, string imageData)
		{
			try
			{
				sender.Send(new OscMessage("/image/set", projectID.ToString(), imageID.ToString(), imageData));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) ImageSet: " + e.Message);
			}
		}

		public void SoundfileVersion(Guid projectID, Guid soundfileID, int version)
		{
			try
			{
				sender.Send(new OscMessage("/soundfile/version", projectID, soundfileID, version));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) SoundfileVersion: " + e.Message);
			}
		}

		public void SoundfileSet(Guid projectID, Guid soundfileID, string soundfileData)
		{
			try
			{
				sender.Send(new OscMessage("/soundfile/set", projectID.ToString(), soundfileID.ToString(), soundfileData));
			}
			catch (Exception e)
			{
				Global.Log.AddEntry("(Server) SoundfileSet: " + e.Message);
			}
		}

		static string ToBlob(string data)
		{
			byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(data);
			string base64 = System.Convert.ToBase64String(bytes);
			return "{ blob: " + base64 + "}";
		}
	}
}
