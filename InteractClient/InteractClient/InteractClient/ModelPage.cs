using InteractClient.Data;
using InteractClient.JintEngine;
using InteractClient.Network;
using Jint.Runtime.Interop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace InteractClient
{
  public class ModelPage : ContentPage
  {

    public Implementation.UI.Grid PageRoot = null;

    public ModelPage()
    {
      NavigationPage.SetHasNavigationBar(this, false);
      PageRoot = new Implementation.UI.Grid();
      Content = PageRoot.InternalObject as Grid;
      //PageRoot.BackgroundColor = Data.Project.Current.ConfigPage.Color.Get();
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();
      Global.CurrentPage = this;
      Engine.Instance.SetActivePage(this);
    }

    // This method can be called from a script to switch to another script
    public void StartScript(string scriptName)
    {
      (PageRoot.InternalObject as Xamarin.Forms.Grid).Children.Clear();

      //PageRoot.BackgroundColor = Data.Project.Current.ConfigPage.Color.Get();

      Screen screen = Data.Project.Current.GetScreen(scriptName);
      if (screen != null)
      {
        Device.BeginInvokeOnMainThread(() =>
        {
          Engine.Instance.StartScript(screen.ID);
        }
        );
      }
    }

    public void Pop()
    {
      if (Navigation.NavigationStack.Count > 1) Navigation.PopAsync();
    }

    protected override bool OnBackButtonPressed()
    {
      Engine.Instance.StopScreen();
      Network.Sender.Get().Disconnect();
      //Navigation.PopToRootAsync();
      return base.OnBackButtonPressed();
    }
  }
}
