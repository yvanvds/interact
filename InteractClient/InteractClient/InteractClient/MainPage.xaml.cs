using Acr.Settings;
using InteractClient.Network;
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
        Multicast multicast;
        Network.Network network;
        Service hub;

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            multicast = Multicast.Get();
            network = Network.Network.Get();
            hub = Network.Service.Get();

            this.FindByName<Entry>("UserName").Text = Settings.Current.Get<String>("UserName");

            UpdateServerList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.CurrentPage = this;
        }

        public void UpdateServerList()
        {
            ServerList.ItemsSource = null;
            ServerList.ItemsSource = Server.Servers;
            if(Server.Servers.Count > 0)
            {
                PageGrid.RowDefinitions[0].Height = 0;
            } else
            {
                PageGrid.RowDefinitions[0].Height = 100;
            }
        }

        private void ButtonServer_Clicked(object sender, EventArgs e)
        {
            Server.Servers.Clear();
            UpdateServerList();
            Settings.Current.Set<String>("UserName", this.FindByName<Entry>("UserName").Text);
            multicast.RequestServerList();
            Logo.RotateTo(1000, 3000);
        }

        private async void Server_Tapped(object sender, EventArgs e)
        {

            if (sender is ViewCell)
            {
                ViewCell cell = sender as ViewCell;
                Server server = cell.BindingContext as Server;
                Logo.RotateTo(500, 3000);
                await hub.ConnectAsync(server);
            }
        }
    }
}
