using Acr.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Network
{
	public class Sender
	{
		private static Sender instance = null;

		private Osc.OscSender sender;

		private string serverName;
		public string ServerName => serverName;

		public string ServerAddress => sender.Host;

		public Implementation.Network.Clients Clients = new Implementation.Network.Clients();

		public static Sender Get()
		{
			if(instance == null)
			{
				instance = new Sender();
			}
			return instance;
		}

		public Sender()
		{
			sender = new Osc.OscSender();
		}

		public void Init(string name, string address, int port)
		{
			serverName = name;
			sender.Init(address, port);
		}

		

		public void Connect()
		{
			sender.Send(new Osc.OscMessage("/client/register", Global.deviceID.ToString(), CrossSettings.Current.Get<String>("UserName")));
		}

		public void Disconnect()
		{
			Global.Connected = false;
			sender.Send(new Osc.OscMessage("/client/disconnect", Global.deviceID.ToString()));
		}

		public void GetNextMethod()
		{
			sender.Send(new Osc.OscMessage("/client/get/nextmethod", Global.deviceID.ToString()));
		}

		public void ProjectUpdateReady(Guid projectID)
		{
			sender.Send(new Osc.OscMessage("/client/projectready", Global.deviceID.ToString(), projectID.ToString()));
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
			sender.Send(new Osc.OscMessage("/client/ping", Global.deviceID.ToString()));
		}

		/* 
		 * send to other clients
		 */ 

		public void SendToClient(Guid clientID, string address, params object[] arguments)
		{
			sender.Send(new Osc.OscMessage("/proxy", clientID.ToString(), address, arguments));
		}

		public void InvokeMethod(Guid clientID, string method, params object[] arguments)
		{
			SendToClient(clientID, "/action/invoke", method, arguments);
		}

		public void StartScreen(Guid clientID, Guid screenID)
		{
			SendToClient(clientID, "/screen/start", screenID.ToString());
		}

	}
}
