using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace InteractClient.Droid
{
  [Activity(Label = "InteractClient", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      TabLayoutResource = Resource.Layout.Tabbar;
      ToolbarResource = Resource.Layout.Toolbar;

      base.OnCreate(bundle);

      Global.Yse = new YSE.YseInterface(OnLogMessage);
			Global.Yse.Log.Level = IYse.ERROR_LEVEL.DEBUG;

			global::Xamarin.Forms.Forms.Init(this, bundle);

      Xamarin.Forms.DependencyService.Register<Implementation.SensorImplementation>();
      LoadApplication(new App());
    }

    private void OnLogMessage(string message)
    {
      //Network.Signaler.Get().WriteLog("Yse Sound Engine: " + message);
			System.Diagnostics.Debug.WriteLine("Yse Sound Engine: " + message);
		}
  }
}

