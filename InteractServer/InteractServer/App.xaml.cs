using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;

namespace InteractServer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public Network.FileServer fileServer = new Network.FileServer();
		public Network.Multicast MulticastServer = new Network.Multicast();
		public Network.OscReceiver OscReceiver = new Network.OscReceiver();
		public Network.Resolume Resolume = new Network.Resolume();
		public Clients.ClientList ClientList = new Clients.ClientList();

		private Timer timer = new Timer();

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			Osc.Tree.Init();

			MulticastServer.Start();

			timer.Elapsed += new ElapsedEventHandler(OnNetworkTimerEvent);
			timer.Interval = 10000;
			timer.Enabled = true;
		}

		private void OnNetworkTimerEvent(object sender, ElapsedEventArgs e)
		{
			ClientList.Update();
			ClientList.PingAllClients();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			timer.Enabled = false;
			ClientList.CloseConnections();

			Resolume.DisConnect();
			OscReceiver.Stop();
			MulticastServer.Stop();
			fileServer.Stop();
			if (Project.Project.Current != null)
			{
				if(Project.Project.Current.NeedsSaving())
					Project.Project.Current.Save();	
			}

			if (Yse.Yse.Handle != null)
			{
				Yse.Yse.Handle.Dispose();
			}

			base.OnExit(e);
		}
	}
}
