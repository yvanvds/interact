using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace InteractClient
{
	public partial class App : Application
	{
		bool yseIsActive = false;

		public App ()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new MainPage());
			NavigationPage.SetHasNavigationBar(MainPage, false);


		}

		protected override void OnStart ()
		{
			Global.OscLocal = new OscTree.Tree(new OscTree.Address("LocalClient", "LocalClient"));
			Global.OscServer = new OscTree.Tree(new OscTree.Address("Server", "Server"));
			Global.OscAllClients = new OscTree.Tree(new OscTree.Address("AllClients", "AllClients"));
			Global.OscRoot.Add(Global.OscLocal);
			Global.OscRoot.Add(Global.OscServer);
			//Global.OscRoot.ErrorHandler += Network.Sender.WriteLog;
			//Global.OscLocal.ErrorHandler += Network.Sender.WriteLog;
			//Global.OscServer.ErrorHandler += Network.Sender.WriteLog;
			//Global.OscAllClients.ErrorHandler += Network.Sender.WriteLog;

			Global.OscServer.ReRoute += Network.Sender.ToServer;
			Global.OscAllClients.ReRoute += Network.Sender.ToServer;

			Global.OscServer.DataTag = Global.deviceID;
			Global.OscAllClients.DataTag = Global.deviceID;
			Global.OscLocal.Add(Global.ClientScripts);

			if(Global.Yse != null)
			{
				Global.Yse.System.Init();
				Global.Yse.System.AutoReconnect(true, 20);
				System.Diagnostics.Debug.WriteLine("Yse Version: " + Global.Yse.System.Version + "\n");
				yseIsActive = true;
				TimeSpan time = new TimeSpan(0, 0, 0, 0, 50);
				Device.StartTimer(time, UpdateYse);
			}
			
			if(Device.RuntimePlatform == Device.UWP)
			{
				Global.Arduino = new Arduino.Arduino(Global.OscLocal);
			}
		}

		protected override void OnSleep ()
		{
			if(Global.Yse != null)
			{
				yseIsActive = false;
				Global.Yse.System.Close();
			}
			
		}

		protected override void OnResume ()
		{
			if(Global.Yse != null && !yseIsActive)
			{
				yseIsActive = true;
				TimeSpan time = new TimeSpan(0, 0, 0, 0, 50);
				Device.StartTimer(time, UpdateYse);
				Global.Yse.System.Resume();
			}
		}

		bool UpdateYse()
		{
			if (Global.Yse == null) return false;
			if (!yseIsActive) return false;
			Global.Yse.System.Update();


			int missed = Global.Yse.System.MissedCallbacks();
			if (missed > 0)
			{
				System.Diagnostics.Debug.WriteLine("Number of callbacks missed: " + missed.ToString());
			}

			return true;
		}
	}
}
