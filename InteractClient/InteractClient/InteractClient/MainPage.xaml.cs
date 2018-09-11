using Acr.Settings;
using InteractClient.Network;
using Microsoft.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

			CrossSettings.Current.Remove("Guid");
			if (!CrossSettings.Current.Contains("Guid"))
			{
				CrossSettings.Current.Set<string>("Guid", shortid.ShortId.Generate(true));
			}
			Global.deviceID = CrossSettings.Current.Get<string>("Guid");

			Network.Udp.Start();
			Receiver.Get().Start();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			Global.CurrentPage = this;
			Servers.Refresh();
		}

		private void AboutButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Pages.About());
		}

		private void ConnectButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Pages.Servers());
		}

		private void ConfigButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Pages.Options());
		}

		private void ProjectsButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Pages.Projects());
		}

	}
}
