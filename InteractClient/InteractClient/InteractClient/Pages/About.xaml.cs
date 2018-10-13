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
	public partial class About : ContentPage
	{
		public About ()
		{
			InitializeComponent ();

			Par1.Text =
				"Interact is an app for live performances. With the Interact server application in your network, it is easy to communicate with other interact clients. Servers also allow you to join projects which define the available interactions.";
			Par3.Text =
				"All device input can be used with interact, be it touch input or sensors. The current project defines if the input is used locally, for instance to generate sound, or sent to another networked device, using OSC.";
			Par2.Text =
				"Server Projects can change client behaviour at all times, show screen messages, images or play sounds.";
			Par4.Text =
				"For more information, visit the Interact website on https://interact.mutecode.com.";
			Par5.Text =
				"Interact is funded by Musica, Impulse Centre for Music, and by C-TAKT.";
		}

		private void BackButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PopAsync();
		}

		private void SiteButton_Clicked(object sender, EventArgs e)
		{
			Device.OpenUri(new Uri("https://interact.mutecode.com"));
		}
	}
}