﻿using Acr.Settings;
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
  public partial class OptionsPage : ContentPage
  {
    public OptionsPage()
    {
      InitializeComponent();
      NavigationPage.SetHasNavigationBar(this, false);

      this.FindByName<Entry>("IDEntry").Text = Settings.Current.Get<String>("UserName");
      this.FindByName<Entry>("TokenEntry").Text = Settings.Current.Get<String>("NetworkToken");
    }

    private void Back_Clicked(object sender, EventArgs e)
    {
      Global.LookForServers = true;
      Navigation.PopAsync();
    }

    private void IDEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
      Settings.Current.Set<String>("UserName", this.FindByName<Entry>("IDEntry").Text);
    }

    private void TokenEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
      Settings.Current.Set<String>("NetworkToken", this.FindByName<Entry>("TokenEntry").Text);
    }

    private void ArduinoButton_Clicked(object sender, EventArgs e)
    {
      switch(Device.RuntimePlatform)
      {
        case Device.Windows:
        case Device.WinPhone:
          Navigation.PushAsync(new ArduinoPage());
          break;
        default:
          DisplayAlert("Alert", "Arduino connections are not available on this platform", "OK");
          break;
      }
    }
  }
}