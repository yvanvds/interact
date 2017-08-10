using InteractClient.JintEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InteractClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectedPage : ContentPage
    {
        Network.Network network;

        public ConnectedPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            network = Network.Network.Get();
        }

        public void SetActivity(string message, bool rotate = true)
        {
            ActivityText.Text = message;
            Logo.RelRotateTo(100);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.CurrentPage = this;
            Engine.Instance.SetActivePage(this);
            SetActivity("Connected To " + InteractClient.Network.Signaler.Get().ConnectedServer?.Name, false);
        }



        private void DisconnectButton_Clicked(object sender, EventArgs e)
        {
            Network.Signaler.Get().Disconnect();
            Navigation.PopToRootAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            Network.Signaler.Get().Disconnect();
            return base.OnBackButtonPressed();
        }

        public void PushModelPage()
        {
            Navigation.PushAsync(new ModelPage());
        }
    }
}
