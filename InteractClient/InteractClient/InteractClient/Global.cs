using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient
{
	public static class Global
	{
		public const int UdpPort = 11012;
		public const int TcpPort = 11013;
		public const int MulticastPort = 33344;
		public const String MulticastAddress = "239.192.0.1";

		public static ContentPage CurrentPage = null;
		public static string deviceID;

		public static Settings Settings = new Settings();

		public static Project.Project CurrentProject { get; set; } = null;
		public static Dictionary<string, Project.Project> ProjectList = new Dictionary<string, Project.Project>();

		public static OscTree.Tree OscRoot = new OscTree.Tree(new OscTree.Address("Root", "Root"));
		public static OscTree.Tree OscLocal;
		public static OscTree.Tree OscAllClients;
		public static OscTree.Tree OscServer;
		public static OscTree.Object ClientScripts = new OscTree.Object(new OscTree.Address("ClientScripts", "ClientScripts"), typeof(Object));

		public static IYse.IYseInterface Yse;

		public static Sensors.ISensor Sensors;

		public static Arduino.Arduino Arduino;

		public static ICompiler Compiler;

		public enum NetworkMessage
		{
			EndOfMessage, // sent by client and server to indicate end of message
			Acknowledge, // sent by server to acknowledge presence
		}

		public static void SetScreenMessage(string message)
		{
			if(CurrentPage != null && CurrentPage is Pages.Connected)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					(CurrentPage as Pages.Connected).SetScreenMessage(message);
				});
			}
		}

		public static void UpdatePage()
		{
			if (CurrentPage is MainPage && Network.Sender.Connected)
			{
				Device.BeginInvokeOnMainThread(() => CurrentPage.Navigation.PushAsync(new Pages.Connected()));
			}
			else if (CurrentPage is Pages.Servers && Network.Sender.Connected)
			{
				Device.BeginInvokeOnMainThread(() => CurrentPage.Navigation.PushAsync(new Pages.Connected()));
			}

			else if (CurrentPage is Pages.Connected && !Network.Sender.Connected)
			{
				Device.BeginInvokeOnMainThread(() => CurrentPage.Navigation.PopToRootAsync());
			}
			else if (CurrentPage is Pages.Performer && !Network.Sender.Connected)
			{
				Device.BeginInvokeOnMainThread(() => CurrentPage.Navigation.PopToRootAsync());
			}
		}

		public static void SetClientGui(OSCGui_Forms.OscGuiView view)
		{
			if(CurrentPage is Pages.Performer)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					(CurrentPage as Pages.Performer).SetView(view);
				});
			}
			else
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					var page = new Pages.Performer(view);
					CurrentPage.Navigation.PushAsync(page);
					CurrentPage = page;
				});
			}
			
		}

		public static void StopClientGui()
		{
			if(CurrentPage is Pages.Performer)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					(CurrentPage as Pages.Performer).Stop();
				});
			}

			if(CurrentProject != null)
			{
				CurrentProject.Stop();
			}
		}

		public static async Task RunOnGui(Action action)
		{
			Device.BeginInvokeOnMainThread(action);
		}
	}
}
