using Acr.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InteractClient.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Connected : ContentPage
	{
		public Connected ()
		{
			InitializeComponent ();

			NavigationPage.SetHasNavigationBar(this, false);

			DisplayClientName.Text = CrossSettings.Current.Get<String>("UserName");
			DisplayClientGuid.Text = Global.deviceID.ToString();
			DisplayClientIP.Text = Network.Udp.GetLocalIPAddress();
			DisplayServerName.Text = Network.Sender.ServerName;
			DisplayServerIP.Text = Network.Sender.ServerAddress;
		}

		public void SetScreenMessage(string message)
		{
			ActivityText.Text = message;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			Global.CurrentPage = this;
			SetScreenMessage("Connected To " + InteractClient.Network.Sender.ServerName);
		}



		private void DisconnectButton_Clicked(object sender, EventArgs e)
		{
			Network.Sender.Disconnect();
			Navigation.PopAsync();
		}

		protected override bool OnBackButtonPressed()
		{
			Network.Sender.Disconnect();
			return base.OnBackButtonPressed();
		}

		public void PushModelPage()
		{
			//Navigation.PushAsync(new ModelPage());
		}
	}
}