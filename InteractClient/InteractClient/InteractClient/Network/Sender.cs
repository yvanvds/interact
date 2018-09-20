using Acr.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Network
{
	public static class Sender
	{
		private static Osc.OscSender sender = new Osc.OscSender();

		private static Server currentServer = null;
		public static string ServerName => currentServer.Name;
		public static string ServerAddress => currentServer.Address;

		private static bool connected = false;
		public static bool Connected => connected;

		public static void Connect(Server server, int port)
		{
			currentServer = server;
			sender.Init(server.Address, port);
			sender.Send(new Osc.OscMessage("/internal/register", Global.deviceID, CrossSettings.Current.Get<String>("UserName")));
			connected = true;
		}

		public static void Disconnect()
		{
			connected = false;
			sender.Send(new Osc.OscMessage("/internal/disconnect", Global.deviceID));
		}

		public static void ServerLost()
		{
			connected = false;
		}

		public static void GetNextMethod()
		{
			sender.Send(new Osc.OscMessage("/internal/get/nextmethod", Global.deviceID));
		}

		public static void ProjectUpdateReady(string projectID)
		{
			sender.Send(new Osc.OscMessage("/internal/project/ready", Global.deviceID, projectID));
		}

		public static void WriteLog(string message)
		{
			sender.Send(new Osc.OscMessage("/internal/log", message));
		}

		public static void Ping()
		{
			sender.Send(new Osc.OscMessage("/internal/ping", Global.deviceID));
		}

		public static void ToServer(OscTree.Route route, object[] parms)
		{
			sender.Send(new Osc.OscMessage("/route" + route.GetActualRoute(), parms));
		}
	}
}
