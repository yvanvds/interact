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
		public static Guid deviceID;

    public static bool LookForServers = false;
    public static bool ConfigPageActive = false;
		public static bool Connected { get; set; } = false;

		public static void UpdatePage()
    {
      if (CurrentPage is MainPage && Connected)
      {
        Device.BeginInvokeOnMainThread(() => CurrentPage.Navigation.PushAsync(new ConnectedPage()));
      }

      else if (CurrentPage is ConnectedPage && !Connected)
      {
        Device.BeginInvokeOnMainThread(() => CurrentPage.Navigation.PopToRootAsync());
      }
    }

    public static IYse.IYseInterface Yse;
  }
}
