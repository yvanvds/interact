using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace InteractClient
{
	public partial class App : Application
	{
		bool yseIsActive = false;

		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new InteractClient.MainPage());
			NavigationPage.SetHasNavigationBar(MainPage, false);
		}

		protected override void OnStart()
		{
			Global.Yse.System.Init();
			Global.Yse.System.AutoReconnect(true, 20);
			System.Diagnostics.Debug.WriteLine("Yse Version: " + Global.Yse.System.Version + "\n");
			yseIsActive = true;
			TimeSpan time = new TimeSpan(0, 0, 0, 0, 50);
			Device.StartTimer(time, UpdateYse);
		}

		protected override void OnSleep()
		{
			yseIsActive = false;
			Global.Yse.System.Close();
		}

		protected override void OnResume()
		{
			yseIsActive = true;
			TimeSpan time = new TimeSpan(0, 0, 0, 0, 50);
			Device.StartTimer(time, UpdateYse);
			Global.Yse.System.Resume();
		}

		bool UpdateYse()
		{
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
