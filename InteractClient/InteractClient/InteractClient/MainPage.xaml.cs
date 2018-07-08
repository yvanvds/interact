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
		Network.Sender sender;

		public MainPage()
    {
      InitializeComponent();
      NavigationPage.SetHasNavigationBar(this, false);

			//CrossSettings.Current.Remove("Guid");
			if(CrossSettings.Current.Contains("Guid")) {
				Global.deviceID = CrossSettings.Current.Get<Guid>("Guid");
			}

      multicast = Multicast.Get();
      network = Network.Network.Get();
			sender = Network.Sender.Get();
			Receiver.Get().Start();

      //this.FindByName<Entry>("UserName").Text = Settings.Current.Get<String>("UserName");
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();
      Global.CurrentPage = this;
      Global.LookForServers = true;

      Device.StartTimer(TimeSpan.FromSeconds(30), () =>
      {
        if(Global.LookForServers == true)
        {
          Network.ServerList.Servers.Clear();
          UpdateServerList();
          Logo.RotateTo(1000, 3000);
          multicast.RequestServerList();
        } else if(!Global.ConfigPageActive)
        {
          if(!Global.Connected)
          {
            Navigation.PopToRootAsync();
            Network.ServerList.Servers.Clear();
            UpdateServerList();
            Global.LookForServers = true;
          }
        }
        return true;
      });
    }

    public void UpdateServerList()
    {
      ServerLst.ItemsSource = null;
      ServerLst.ItemsSource = Network.ServerList.Servers;
      if (Network.ServerList.Servers.Count > 0)
      {
        PageGrid.RowDefinitions[0].Height = 0;
      }
      else
      {
        PageGrid.RowDefinitions[0].Height = 100;
      }
    }

    private void ButtonServer_Clicked(object sender, EventArgs e)
    {
      Network.ServerList.Servers.Clear();
      UpdateServerList();
      multicast.RequestServerList();
      Logo.RotateTo(1000, 3000);
    }

    private void Server_Tapped(object sender, EventArgs e)
    {

      if (sender is ViewCell)
      {
        ViewCell cell = sender as ViewCell;
        ServerList server = cell.BindingContext as ServerList;
        Logo.RotateTo(500, 3000);
        Global.LookForServers = false;
				this.sender.Init(server.Name, server.Address, 11234);
				this.sender.Connect();
      }
    }

    private void UserName_TextChanged(object sender, TextChangedEventArgs e)
    {
      CrossSettings.Current.Set<String>("UserName", this.FindByName<Entry>("UserName").Text);
    }

    private void OptionsButton_Clicked(object sender, EventArgs e)
    {
      Global.LookForServers = false;
      Global.ConfigPageActive = true;
      Navigation.PushAsync(new OptionsPage());
    }
  }
}
