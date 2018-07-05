using Acr.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Network
{
	public class Sender
	{
		private Osc.OscSender sender;

		public Sender()
		{
			sender = new Osc.OscSender();
		}

		public void Init(string address, int port)
		{
			sender.Init(address, port);
		}

		public void Connect()
		{
			sender.Send(new Osc.OscMessage("/client/register", CrossSettings.Current.Get<String>("UserName")));
		}

		public void Disconnect()
		{
			sender.Send(new Osc.OscMessage("/client/disconnect"));
		}

		public void GetNextMethod()
		{
			sender.Send(new Osc.OscMessage("/client/nextmethod"));
		}

		public void GetProjectConfig(Guid projectID)
		{
			sender.Send(new Osc.OscMessage("/client/get/projectconfig", projectID.ToString()));
		}

		public void GetScreen(Guid projectID, Guid screenID)
		{
			sender.Send(new Osc.OscMessage("/client/get/screen", projectID.ToString(), screenID.ToString()));
		}

		public void GetImage(Guid projectID, Guid imageID)
		{
			sender.Send(new Osc.OscMessage("/client/get/image", projectID.ToString(), imageID.ToString()));
		}

		public void GetSoundfile(Guid projectID, Guid soundfileID)
		{
			sender.Send(new Osc.OscMessage("/client/get/soundfile", projectID.ToString(), soundfileID.ToString()));
		}

		public void GetPatcher(Guid projectID, Guid patcherID)
		{
			sender.Send(new Osc.OscMessage("/client/get/patcher", projectID.ToString(), patcherID.ToString()));
		}

		public void WriteLog(string message)
		{
			sender.Send(new Osc.OscMessage("/server/log", message));
		}

		public void WriteErrorLog(int index, int lineNumber, string message, Guid resourceID)
		{
			sender.Send(new Osc.OscMessage("/server/errorlog", index, lineNumber, message, resourceID.ToString()));
		}

		public void InvokeMethod(string method, params object[] arguments)
		{
			sender.Send(new Osc.OscMessage("/server/invoke", method, arguments));
		}

		public void Ping()
		{
			sender.Send(new Osc.OscMessage("/server/ping"));
		}

		/* 
		 * send to other clients
		 */ 

		public void SendToClient(string clientID, string address, params object[] arguments)
		{
			sender.Send(new Osc.OscMessage("/proxy", clientID, address, arguments));
		}

		public void InvokeMethod(string clientID, string method, params object[] arguments)
		{
			SendToClient(clientID, "/action/invoke", method, arguments);
		}

		public void StartScreen(string clientID, Guid screenID)
		{
			SendToClient(clientID, "/screen/start", screenID.ToString());
		}

	}
}
