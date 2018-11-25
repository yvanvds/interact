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
	public partial class Options : ContentPage
	{
		public Options ()
		{
			InitializeComponent ();

			this.FindByName<Entry>("IDEntry").Text = Global.Settings.ID;
			this.FindByName<Entry>("TokenEntry").Text = Global.Settings.Token;

			if (Device.RuntimePlatform != Device.UWP)
			{
				ArduinoButton.IsVisible = false;
				BackButton.IsVisible = false;
			}
		}

		private void IDEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			Global.Settings.ID = this.FindByName<Entry>("IDEntry").Text;
		}

		private void TokenEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			Global.Settings.Token = this.FindByName<Entry>("TokenEntry").Text;
		}

		private void ArduinoButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Arduino());
		}

		private void BackButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PopAsync();
		}
	}
}