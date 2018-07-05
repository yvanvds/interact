using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient
{
  public static class Global
  {
    public static ContentPage CurrentPage = null;

    public static bool LookForServers = false;
    public static bool ConfigPageActive = false;

    public static void UpdatePage(bool connected)
    {
      if (CurrentPage is MainPage && connected)
      {
        Device.BeginInvokeOnMainThread(() => CurrentPage.Navigation.PushAsync(new ConnectedPage()));
      }

      else if (CurrentPage is ConnectedPage && !connected)
      {
        Device.BeginInvokeOnMainThread(() => CurrentPage.Navigation.PopToRootAsync());
      }
    }

    public static IYse.IYseInterface Yse;
  }
}
