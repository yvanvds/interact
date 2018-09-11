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

			this.FindByName<Entry>("IDEntry").Text = CrossSettings.Current.Get<String>("UserName");
			this.FindByName<Entry>("TokenEntry").Text = CrossSettings.Current.Get<String>("NetworkToken");

			if (Device.RuntimePlatform != Device.UWP)
			{
				ArduinoButton.IsEnabled = false;
			}
		}

		private void IDEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			CrossSettings.Current.Set<String>("UserName", this.FindByName<Entry>("IDEntry").Text);
		}

		private void TokenEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			CrossSettings.Current.Set<String>("NetworkToken", this.FindByName<Entry>("TokenEntry").Text);
		}

		private void ArduinoButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Arduino());
		}
	}
}