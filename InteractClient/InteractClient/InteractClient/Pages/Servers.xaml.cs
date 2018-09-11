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
	public partial class Servers : ContentPage
	{
		public Servers ()
		{
			InitializeComponent ();
			ServerList.ItemsSource = Network.Servers.List;
			Global.CurrentPage = this;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			Global.CurrentPage = this;
		}

		private void RefreshButton_Clicked(object sender, EventArgs e)
		{
			Network.Servers.Refresh();
		}

		private void Server_Tapped(object sender, EventArgs e)
		{
			if (sender is ViewCell)
			{
				ViewCell cell = sender as ViewCell;
				var server = cell.BindingContext as Network.Server;
				Network.Sender.Connect(server, 11234);
			}

		}
	}
}